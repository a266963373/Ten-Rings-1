using System;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public int Id;
    public string Name;     // used for index
    public int Power = 5;    // for status' effect strength
    public bool IsBuff;               // 是否为正面效果
    public bool IsStackable;
    public StatusDecayTrigger DecayTrigger;
    public StatType DecayStatType;
    public List<StatModifier> StatModifiers = new();
    public List<TriggerEffect> TriggerEffects = new();
    public List<StatusDynamicStatMod> DynamicStatMods = new();
    //public Action OnApplyEffect;  // effect when applied

    // run time attributes
    public float Stack = 0;
    public int EffectiveStack => (int)Mathf.Max(Stack, 1);  // an integer that's at least 1
    public Character Applier; // who applied this status
    public Character Bearer; // who has this status

    public Action OnWorldTurnEffect;
    public Action OnCharacterTurnEffect;

    public void OnTurn(StatusDecayTrigger statusDecayTrigger, Character c)
    {
        if (statusDecayTrigger == StatusDecayTrigger.OnWorldTurn)
        {
            OnWorldTurnEffect?.Invoke();
        }
        else if (statusDecayTrigger == StatusDecayTrigger.OnCharacterTurnBegin)
        {
            OnCharacterTurnEffect?.Invoke();
        }
    }
}
