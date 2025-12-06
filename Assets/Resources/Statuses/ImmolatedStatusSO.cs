using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/ImmolatedStatusSO")]
public class ImmolatedStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        int burningStatusId = 8;

        status.OnApplyEffect = () =>
        {
            Status burningStatus = status.Bearer.StatusSystem.GetStatusById(burningStatusId);
            if (burningStatus == null)
            {
                ActionResolver.I.ApplyStatusByName(status.Bearer, status.Bearer, "Burning", 1);
            }
            else if (burningStatus.Stack < 1)
            {
                burningStatus.Stack = 1;
            }
        };

        status.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnAfterStatusStackChanged,
            EffectOnStatus = (Status s) =>
            {
                if (s.Id == burningStatusId && s.Stack < 1)
                {
                    s.Stack = 1;
                }
            }
        });

        status.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnAfterMeleeAction,
            Effect = (BattleAction action) =>
            {
                Status burningStatus = action.Actor.StatusSystem.GetStatusById(burningStatusId);
                ActionResolver.I.ApplyStatusByName(action.Actor, action.Target, "Burning", status.Stack);
            }
        });

        status.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnAfterMeleeTargeted,
            Effect = (BattleAction action) =>
            {
                Status burningStatus = action.Target.StatusSystem.GetStatusById(burningStatusId);
                ActionResolver.I.ApplyStatusByName(action.Target, action.Actor, "Burning", status.Stack);
            }
        });
    }
}
