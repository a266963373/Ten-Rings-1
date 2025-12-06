using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/BipolarSprayedStatusSO")]
public class BipolarSprayedStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnAfterTakeDamage,
            Effect = (ba) =>
            {
                if (ba.Damage.Element == Element.Fire || ba.Damage.Element == Element.Ice)
                {
                    var statusName = ba.Damage.Element == Element.Fire ? "Burning" : "Frozen";
                    ActionResolver.I.ApplyStatusByName(null, ba.Target, statusName, status.Stack);
                    status.Bearer.StatusSystem.RemoveStatus(status);
                }
            }
        });
    }
}
