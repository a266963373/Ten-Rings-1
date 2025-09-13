using System;
using System.Collections.Generic;
using UnityEngine.Localization.PropertyVariants.TrackedProperties; // for Action

public class BattleAction   // it's BattleActionSO + actor and targets
{
    public string Name;
    public Character Actor;
    public Character Target;
    public Damage Damage;
    public Status Status;
    public StatType Scale;
    public int Power = 100; // multiply with dmg/status power
    public AreaType Area;
    public int ManaCost;
    public bool HasExtraLog = false;
    public BattleAction RelatedAction = null;
    public bool IsDoRelatedActionInstead = false;
    public bool IsMissed = false;
    public bool IsCrit = false;
    public bool IsBlocked = false;
    public StatModifier ActorTempStatMod;
    public bool IsDamage = true;
    public bool IsStatus;

    public float scaledValue; // store the calculated damage/status value after scaling

    // 新增：行动后回调（用于像吞噬这种结算后判断的效果）
    public List<TriggerEffect> TriggerEffects = new();

    public void Trigger(TriggerType triggerType)    // eg. for update shield value
    {
        foreach (var te in TriggerEffects)
        {
            if (te.Trigger == triggerType)
            {
                te.Effect?.Invoke(this);
            }
        }
    }
}
