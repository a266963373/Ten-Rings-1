using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/FieryPursuedStatusSO")]
public class FieryPursuedStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect
        {
            Timing = TimingType.OnWorldTurn,
            EffectOnTime = () =>
            {
                ActionResolver.I.ApplyStatusByName(status.Applier, status.Bearer,
                    "Burning", status.Power);
            }
        });
    }
}
