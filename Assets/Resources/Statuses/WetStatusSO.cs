using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/WetStatusSO")]
public class WetStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TakeDamageModifier = (Damage damage) =>
        {
            if (damage.Element == Element.Fire)
            {
                damage.Value -= status.CommonEffectValue();
            }
            else if (damage.Element == Element.Electric || 
                damage.Element == Element.Ice)
            {
                damage.Value += status.CommonEffectValue();
            }
            return damage;
        };

        status.OnApplyEffect = () =>
        {
            var ss = status.Bearer.StatusSystem;
            var burningStatus = ss.GetStatusByName("Burning");
            if (burningStatus != null)
            {
                var deductStack = Mathf.Min(burningStatus.Stack, status.Stack);
                ss.ModifyStatus(burningStatus, -deductStack);
                ss.ModifyStatus(status, -deductStack);
            }
        };
    }
}
