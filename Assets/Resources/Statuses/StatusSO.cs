using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusDecayTrigger
{
    OnWorldTurn,
    OnCharacterTurnBegin,
    OnCharacterTurnEnd,
    // that's it for now
}

[CreateAssetMenu(menuName = "Status/StatusSO")]
public class StatusSO : ScriptableObject
{
    public int Id;
    public string Name;     // used for index
    public int Power = 5;    // for status' effect strength
    public bool IsBuff = false;               // 是否为正面效果
    public float StartStack = 1;
    public bool IsStackable = false;
    public StatusDecayTrigger DecayTrigger = StatusDecayTrigger.OnWorldTurn;
    public StatType DecayStatType = StatType.NON; // if NON, decay by 1 each time

    public Status GetStatus(Character applier, Character bearer)
    {
        Status status = new()
        {
            Id = Id,
            Name = Name,
            Applier = applier,
            Bearer = bearer,
            Power = Power,
            Stack = StartStack,
            IsBuff = IsBuff,
            IsStackable = IsStackable,
            DecayTrigger = DecayTrigger,
            DecayStatType = DecayStatType,
        };
        InitStatus(status);
        return status;
    }

    protected virtual void InitStatus(Status status) { }

    public int EffectValue(int stack)   // for most StatModStatus
    {
        stack = Mathf.Min(stack, 10);
        int last = 11 - stack;
        int dmg = stack * (10 + last) / 2;
        return Mathf.Min(dmg, 50);
    }
}
