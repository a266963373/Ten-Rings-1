using System;
public enum TriggerType
{
    OnBeforeAction,
    OnBeforeTakeDamage,
    OnBeforeDealDamage,
    OnAfterTakeDamage,
    OnAfterDealDamage,
    OnAfterDeath,
    OnBlocking,

    OnAfterResolve,
}

public enum TimingType
{
    OnWorldTurn,
    OnSelfTurn,
}

public class TriggerEffect
{
    public TriggerType Trigger;
    public Action<BattleAction> Effect;
}
