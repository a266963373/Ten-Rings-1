using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/StatusSO")]
public class StatusSO : ScriptableObject
{
    public int Id;
    public string Name;     // used for index
    public int Power = 10;    // for status' effect strength
    public int Power2 = 10;
    public bool IsBuff = false;               // 是否为正面效果
    public float StartStack = 1;    // by default
    public bool IsStackable = false;
    public TimingType DecayTrigger;
    public StatType DecayStatType = StatType.NON; // if NON, decay by 1 each time
    public int UpkeepMana = 0;
    public bool IsApplierMaintained = false; // if true, when applier dies, status is removed

    public Status GetStatus(Character applier, Character bearer)
    {
        Status status = new()
        {
            Id = Id,
            Name = Name,
            Applier = applier,
            Bearer = bearer,
            Power = Power,
            Power2 = Power2,
            Stack = StartStack,
            IsBuff = IsBuff,
            IsStackable = IsStackable,
            DecayTrigger = DecayTrigger,
            DecayStatType = DecayStatType,
            UpkeepMana = UpkeepMana,
        };
        InitStatus(status);

        if (status.DecayTrigger == TimingType.OnApplierTurnBegin)
        {
            status.Applier.OnApplierTurnBegin -= status.Bearer.StatusSystem.OnApplierTurnBegin;
            status.Applier.OnApplierTurnBegin += status.Bearer.StatusSystem.OnApplierTurnBegin;
        }

        if (IsApplierMaintained)
        {
            status.Applier.TriggerEffects.Add(new TriggerEffect()
            {
                Trigger = TriggerType.OnAfterDeath,
                Effect = (ba) => { status.Bearer.StatusSystem.RemoveStatus(status); }
            });
        }

        return status;
    }

    protected virtual void InitStatus(Status status) { }

}
