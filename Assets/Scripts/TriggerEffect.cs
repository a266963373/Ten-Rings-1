using System;
public enum TriggerType
{
    OnBeforeAction,
    OnBeforeTargeted,
    OnBeforeTakeDamage,
    OnBeforeDealDamage,
    OnAfterTakeDamage,
    OnAfterDealDamage,

    OnExtraEffect,

    OnAfterMeleeAction,
    OnAfterMeleeTargeted,
    OnAfterMeleeContact,

    OnAfterAction,

    OnBeforeDeath,
    OnAfterDeath,
    OnBlocking,

    OnAfterResolve,
    OnAfterResolveNoMiss,

    OnNoUpkeepMana,
    OnAfterStatusStackChanged,
    OnAfterStatusRemoved,
}   // when adding new trigger type, remember to update GetTriggerer method

public enum TimingType
{
    OnWorldTurn,
    None,
    OnSelfTurnBegin,
    OnSelfTurnEnd,
    OnApplierTurnBegin,
}

public enum IntModType
{
    ManaCost,
}

public class TriggerEffect
{
    public TriggerType Trigger;
    public TimingType Timing;
    public IntModType IntMod;

    public Action<BattleAction> Effect;
    public Action<Status> EffectOnStatus;
    public Action EffectOnTime;
    public Func<BattleActionSO, int> EffectOnInt;

    public TriggerEffect(TriggerEffect other)
    {
        Trigger = other.Trigger;
        Timing = other.Timing;
        IntMod = other.IntMod;

        Effect = other.Effect;
        EffectOnStatus = other.EffectOnStatus;
        EffectOnTime = other.EffectOnTime;
        EffectOnInt = other.EffectOnInt;
    }

    public TriggerEffect() { }

    public Character GetTriggerer(BattleAction action)
    {
        if (action == null) return null;

        switch (Trigger)
        {
            // 由 Actor 触发
            case TriggerType.OnBeforeAction:
            case TriggerType.OnAfterAction:
            case TriggerType.OnAfterDealDamage:
            case TriggerType.OnBeforeDealDamage:
            case TriggerType.OnAfterMeleeAction:
            case TriggerType.OnAfterResolve:
            case TriggerType.OnAfterResolveNoMiss:
            case TriggerType.OnExtraEffect:
                return action.Actor;

            // 由 Target 触发
            case TriggerType.OnBeforeTargeted:
            case TriggerType.OnBeforeTakeDamage:
            case TriggerType.OnAfterTakeDamage:
            case TriggerType.OnAfterMeleeTargeted:
            case TriggerType.OnAfterMeleeContact:
            case TriggerType.OnBeforeDeath:
            case TriggerType.OnAfterDeath:
            case TriggerType.OnBlocking:
            case TriggerType.OnNoUpkeepMana:
            case TriggerType.OnAfterStatusStackChanged:
            case TriggerType.OnAfterStatusRemoved:
                return action.Target;

            default:
                return null;
        }
    }
}
