using System;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/InvokeActionSO")]
public class InvokeActionSO : BattleActionSO
{
    [NonSerialized] public bool IsActionInvoked = false;
    [NonSerialized] public Character Target;

    protected override void InitAction(BattleAction action)
    {
        if (IsActionInvoked)
        {
            action.IsRemovingStatuses = true;
            action.Target = Target;
            action.Range = RangeType.Global;
            Target = null;
            action.RecoveryTime = 0;
        }
        else
        {
            Target = action.Target;
        }

        action.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnAfterResolveNoMiss,
            Effect = (ba) =>
            {
                IsActionInvoked = !IsActionInvoked;
            }
        });

        action.Actor.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnNoUpkeepMana,
            EffectOnStatus = MaybeRemoveMyStatuses
        });
    }

    private void MaybeRemoveMyStatuses(Status s)
    {
        if (ContainsStatus(s))
        {
            IsActionInvoked = true;
            BattleAction bas = GetAction(s.Applier, Target);
            ActionResolver.I.Resolve(bas);
        }
    }
}
