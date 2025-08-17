using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public event Action OnHpChanged;
    public event Action OnMpChanged;
    public Character Owner;

    public string Explanation = "";

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

    public CharacterStats(CharacterStats other, float statMultiplier = 1)
    {
        foreach (var kv in other.stats)
        {
            stats[kv.Key] = kv.Value;
            if (statMultiplier != 1)
            {
                stats[kv.Key].Value = (int)(kv.Value.Value * statMultiplier);
            }
        }
    }

    public int GetStat(StatType type)
    {
        float modValue = stats[type].Value;

        // 汇总所有戒指的StatModifier
        foreach (var ring in Owner.Rings)
        {
            if (ring == null) continue; // 跳过空戒指
            foreach (var mod in ring.StatModifiers)
                if (mod.StatType == type)
                    modValue = ProcessMod(modValue, mod);
        }

        // 汇总所有状态的StatModifier
        foreach (var status in Owner.StatusSystem.Statuses)
            foreach (var mod in status.StatModifiers)
                if (mod.StatType == type)
                    modValue = ProcessMod(modValue, mod);

        int finalValue = (int)modValue;

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
                Explanation += $"\n × {1 + mod.Value:F2} ({mod.Source})";
                break;
            case ModifierType.Set:
                value = mod.Value;
                Explanation += $"\n ← {mod.Value:F2} ({mod.Source})";
                break;
        }
        return value;
    }

    public void ChangeStat(StatType type, int delta)
    {
        stats[type].Value += delta;
        if (type == StatType.HP || type == StatType.MHP)
        {
            OnHpChanged?.Invoke();
        }
        else if (type == StatType.MP || type == StatType.MMP)
        {
            OnMpChanged?.Invoke();
        }
    }

    public void InitBeforeBattle()
    {
        stats[StatType.HP].Value = GetStat(StatType.MHP);
        stats[StatType.MP].Value = GetStat(StatType.MMP);
    }
}
