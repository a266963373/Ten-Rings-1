using System;
public enum TriggerType
{
    OnAfterTakeDamage,
    OnAfterDealDamage,
    OnAfterDeath,
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

    public TriggerEffect (TriggerType trigger, Action<BattleAction> effect)
    {
        Trigger = trigger;
        Effect = effect;
    }
}
