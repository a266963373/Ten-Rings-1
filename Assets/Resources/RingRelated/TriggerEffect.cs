using System;
public enum TriggerType
{
    OnTakeDamage,
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
