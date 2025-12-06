using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/WaterShieldedStatusSO")]
public class WaterShieldedStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeTakeDamage,
            Effect = (ba) =>
            {
                if (ba.Range == RangeType.Melee || ba.Range == RangeType.Ranged)
                {
                    if (ba.Damage.Element == Element.Fire)
                    {
                        ba.Damage.Value /= 4;
                    }
                    else
                    {
                        ba.Damage.Value /= 2;
                    }
                }
            }
        });
    }
}
