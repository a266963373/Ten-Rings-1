using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/EntangledStatusSO")]
public class EntangledStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeAction,
            Effect = (ba) =>
            {
                if (ba.Range == RangeType.Melee)
                {
                    ba.Actor.Stats.TempStatMods.Add(new StatModifier
                    {
                        Source = status.Name,
                        StatType = StatType.ACC,
                        Value = -status.CommonEffectValue(),
                    });
                }
            }
        });
        status.StatModifiers.Add(new StatModifier
        {
            Source = status.Name,
            StatType = StatType.EVS,
            Value = -status.CommonEffectValue(),
        });
    }
}
