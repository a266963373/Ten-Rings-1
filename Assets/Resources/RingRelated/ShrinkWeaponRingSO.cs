using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ShrinkWeaponRingSO")]
public class ShrinkWeaponRingSO : EnchantmentRingSO
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
                    action.RecoveryTime /= 2;
                    action.Power -= Power;
                }
            });
        };
    }
}
