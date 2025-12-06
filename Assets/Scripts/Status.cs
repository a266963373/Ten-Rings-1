using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Status
{
    public int Id;
    public string Name;     // used for index
    public int Power = 10;    // for status' effect strength
    public int Power2 = 10;
    public bool IsBuff;               // 是否为正面效果
    public bool IsStackable;
    public TimingType DecayTrigger;
    public StatType DecayStatType;
    public int UpkeepMana = 0;
    public List<StatModifier> StatModifiers = new();
    public List<TriggerEffect> TriggerEffects = new();
    public List<StatusDynamicStatMod> DynamicStatMods = new();

    // run time attributes
    public float Stack = 0;
    public int EffectiveStack => (int)Mathf.Max(Stack, 1);  // an integer that's at least 1
    public Character Applier; // who applied this status
    public Character Bearer; // who has this status

    public Action OnApplyEffect;
    public Func<Damage, Damage> DealDamageModifier;
    public Func<Damage, Damage> TakeDamageModifier;

    public override string ToString()
    {
        return Name;
    }

    public int CommonEffectValue(bool isCap100 = true)   // for most StatModStatus
    {
        var usingPower = Power;
        bool isPowerNegative = Power < 0;
        if (isPowerNegative) usingPower = -Power;

        int stack = Mathf.Min(EffectiveStack, 10);
        int last = 10 + 1 - stack;
        int dmg = stack * (usingPower + last);
        if (!isCap100) dmg /= 2;
        dmg = Mathf.Min(dmg, isCap100 ? 100 : 50);
        
        if (isPowerNegative) dmg = -dmg;
        return dmg;
    }

    public Status(Status other)
    {
        Id = other.Id;
        Name = other.Name;
        Power = other.Power;
        IsBuff = other.IsBuff;
        IsStackable = other.IsStackable;
        DecayTrigger = other.DecayTrigger;
        DecayStatType = other.DecayStatType;
        UpkeepMana = other.UpkeepMana;
        Stack = other.Stack;
        Applier = other.Applier;
        Bearer = other.Bearer;
        OnApplyEffect = other.OnApplyEffect;
        DealDamageModifier = other.DealDamageModifier;
        TakeDamageModifier = other.TakeDamageModifier;

        StatModifiers = other.StatModifiers
            .Select(sm => new StatModifier(sm))
            .ToList();

        TriggerEffects = other.TriggerEffects
            .Select(te => new TriggerEffect(te))
            .ToList();

        DynamicStatMods = other.DynamicStatMods
            .Select(dm => new StatusDynamicStatMod(dm))
            .ToList();
    }

    public Status() { }
}
