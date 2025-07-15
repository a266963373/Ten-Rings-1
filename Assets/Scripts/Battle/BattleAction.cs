using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction   // it's BattleActionSO + actor and targets
{
    public string Name;
    public Character Actor;
    public Character Target;
    public Damage Damage;
    public BattleActionTargetType TargetType;
    public int ManaCost;
    public bool HasExtraLog = false;
    public BattleAction RelatedAction = null;
    public bool IsDoRelatedActionInstead = false;

    public BattleAction Resolve()
    {
        // actually do the action
        if (IsDoRelatedActionInstead)
        {
            return RelatedAction.Resolve();
        }

        if (ManaCost != 0)
        {
            Actor.Stats.ChangeStat(StatType.MP, -ManaCost);
        }

        int scale = Actor != null ? Actor.Stats.GetStat(Damage.Scale) : 100;
        float scaledValue = Damage.Value * scale * 0.01f;
        Damage.Value = Mathf.RoundToInt(scaledValue);
        Target.Stats.ChangeStat(StatType.HP, -Damage.Value);

        Actor?.Trigger(TriggerType.OnAfterDealDamage, this);
        Target?.Trigger(TriggerType.OnAfterTakeDamage, this);

        BattleSystem.I.State = BattleState.ActionResolved;
        return this;
    }
}
