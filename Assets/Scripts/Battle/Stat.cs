using System;   // Guid
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatType
{
    NON, MHP, HP, STR, MND, SPD, MMP, MP, 
    EVS, ACC, CRT, BLK, 
    HPR, MPR,
    ALL,
}   // ALL means all stats are affected

public class Stat
{
    public StatType Type;
    public int Value;
}

public enum ModifierType { Flat, Percent, Set }

public class StatModifier
{
    public StatType StatType = StatType.HP;
    public ModifierType ModType = ModifierType.Flat;
    public bool IsPermanent = false;
    public float Value = -1;
    public string Source = "None";
    public Guid Id = Guid.NewGuid();

    // 深拷贝构造函数
    public StatModifier(StatModifier other)
    {
        StatType = other.StatType;
        ModType = other.ModType;
        IsPermanent = other.IsPermanent;
        Value = other.Value;
        Source = other.Source;
        Id = other.Id; // 如果需要新ID可用 Guid.NewGuid()
    }

    public StatModifier() { }

    public bool TypeEquals(StatType otherType)
    {
        return StatType == otherType || StatType == StatType.ALL || otherType == StatType.ALL;
    }
}

public class RingDynamicStatMod
{
    public StatType CheckStatType;
    public Action<Character> Updator;

    // 深拷贝构造函数
    public RingDynamicStatMod(RingDynamicStatMod other)
    {
        CheckStatType = other.CheckStatType;
        Updator = other.Updator;
    }

    public RingDynamicStatMod() { }

    public bool TypeEquals(StatType otherType)
    {
        return CheckStatType == otherType || CheckStatType == StatType.ALL || otherType == StatType.ALL;
    }
}

public class StatusDynamicStatMod
{
    public StatType CheckStatType;
    public Action Updator;

    // 深拷贝构造函数
    public StatusDynamicStatMod(StatusDynamicStatMod other)
    {
        CheckStatType = other.CheckStatType;
        Updator = other.Updator;
    }

    public StatusDynamicStatMod() { }

    public bool TypeEquals(StatType otherType)
    {
        return CheckStatType == otherType || CheckStatType == StatType.ALL || otherType == StatType.ALL;
    }
}