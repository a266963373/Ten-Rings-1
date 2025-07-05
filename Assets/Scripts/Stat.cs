using System;   // Guid
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MHP, HP, STR, SPD 
}

public class Stat
{
    public StatType Type;
    public int Value;
    public List<StatModifier> Mods = new();
    public string Explanation = "";

    public int FinalValue
    {
        get
        {
            float finalValue = Value;
            Explanation = $"Base {Type}: {Value}";

            foreach (var mod in Mods)
            {
                if (mod.ModType == ModifierType.Flat)
                {
                    finalValue += mod.Value;
                    Explanation += $"\n + {mod.Value} ({mod.Source})";
                }
                else if (mod.ModType == ModifierType.Percent)
                {
                    finalValue *= (1 + mod.Value);
                    Explanation += $"\n ¡Á {1 + mod.Value:F2} ({mod.Source})";
                }
            }

            Explanation += $"\n= {finalValue:F0}";
            return (int)finalValue;
        }
    }
}

public enum ModifierType { Flat, Percent }
public class StatModifier
{
    public StatType StatType = StatType.HP;
    public ModifierType ModType = ModifierType.Flat;
    public bool IsPermanent = false;
    public float Value;
    public string Source;
    public Guid Id = Guid.NewGuid();
}

public class CharacterStats
{
    public event Action OnHpChanged;

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
        return stats[type].FinalValue;
    }

    public void AddModifier(StatModifier mod)
    {
        stats[mod.StatType].Mods.Add(mod);
        if (mod.StatType == StatType.HP || mod.StatType == StatType.MHP)
        {
            OnHpChanged?.Invoke();
        }
    }
}
