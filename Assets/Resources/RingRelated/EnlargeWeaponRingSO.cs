using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/EnlargeWeaponRingSO")]
public class EnlargeWeaponRingSO : EnchantmentRingSO
{
    protected override void InitEffect(Ring ring)
    {
        Effect = (ba) =>
        {
            ba.TriggerEffects.Add(new TriggerEffect
            {
                Trigger = TriggerType.OnBeforeAction,
                Effect = (action) =>
                {
                    action.RecoveryTime = Mathf.RoundToInt(action.RecoveryTime * 1.5f);
                    action.Power += Power;
                }
            });
        };
    }
}
