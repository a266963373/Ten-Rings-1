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
}

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
}

public class RingDynamicStatMod
{
    public StatType CheckStatType;
    public Action<Character> Updator;
}

public class StatusDynamicStatMod
{
    public StatType CheckStatType;
    public Action Updator;
}
