using System;   // Guid
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatType
{
    NON, MHP, HP, STR, MND, SPD, MMP, MP
}

public class Stat
{
    public StatType Type;
    public int Value;
    private List<StatModifier> mods = new();
    public string Explanation = "";

    public void AddMod(StatModifier mod)
    {
        if (mod.IsPermanent)
        {
            Value = (int)ProcessMod(Value, mod);
        } else
        {
            mods.Add(mod);
        }
    }

    private float ProcessMod(float value, StatModifier mod)
    {
        switch (mod.ModType)
        {
            case ModifierType.Flat:
                value += mod.Value;
                Explanation += $"\n + {mod.Value} ({mod.Source})";
                break;
            case ModifierType.Percent:
                value *= 1 + mod.Value;
                Explanation += $"\n ¡Á {1 + mod.Value:F2} ({mod.Source})";
                break;
            case ModifierType.Set:
                value = mod.Value;
                Explanation += $"\n ¡û {mod.Value:F2} ({mod.Source})";
                break;
        }
        return value;
    }

    public int FinalValue
    {
        get
        {
            float finalValue = Value;
            Explanation = $"Base {Type}: {Value}";

            foreach (var mod in mods)
            {
                finalValue = ProcessMod(finalValue, mod);
            }

            Explanation += $"\n= {finalValue:F0}";
            return (int)finalValue;
        }
    }
}

public enum ModifierType { Flat, Percent, Set }
public class StatModifier
{
    public StatType StatType;
    public ModifierType ModType;
    public bool IsPermanent;
    public float Value;
    public string Source;
    public Guid Id = Guid.NewGuid();

    public StatModifier(
        StatType statType = StatType.HP, 
        ModifierType modType = ModifierType.Flat, 
        bool isPermanent = false, 
        float value = -1, 
        string source = "None")
    {
        StatType = statType;
        ModType = modType;
        IsPermanent = isPermanent;
        Value = value;
        Source = source;
    }
}

public class CharacterStats
{
    public event Action OnHpChanged;
    public event Action OnMpChanged;

    private Dictionary<StatType, Stat> stats = new();

    public CharacterStats()
    {
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            stats[type] = new()
            {
                Type = type,
                Value = 100,
            };
        }
    }

    public CharacterStats(List<StatEntry> statEntries) : this()
    {
        foreach (StatEntry entry in statEntries)
        {
            stats[entry.Type].Value = entry.Value;
        }
    }

    // when enter battle, copy stats
    public CharacterStats(CharacterStats other)
    {
        foreach (var kv in other.stats)
        {
            stats[kv.Key] = kv.Value;
        }
    }

    public int GetStat(StatType type)
    {
        int finalValue = stats[type].FinalValue;

        if (type == StatType.HP)
        {
            if (finalValue < 0) finalValue = 0;
            else if (finalValue > GetStat(StatType.MHP)) finalValue = GetStat(StatType.MHP);
        }
        else if (type == StatType.MP)
        {
            if (finalValue < 0) finalValue = 0;
            else if (finalValue > GetStat(StatType.MMP)) finalValue = GetStat(StatType.MMP);
        }

        return finalValue;
    }

    public void AddModifier(StatModifier mod)
    {
        stats[mod.StatType].AddMod(mod);
        if (mod.StatType == StatType.HP || mod.StatType == StatType.MHP)
        {
            OnHpChanged?.Invoke();
        }
        else if (mod.StatType == StatType.MP || mod.StatType == StatType.MMP)
        {
            OnMpChanged?.Invoke();
        }
    }
}
