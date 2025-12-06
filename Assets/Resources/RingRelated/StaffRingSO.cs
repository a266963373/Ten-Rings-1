using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/StaffRingSO")]
public class StaffRingSO : WeaponRingSO
{
    protected override void InitRing(Ring ring)
    {
        base.InitRing(ring);
        ring.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnBeforeAction,
            Effect = (BattleAction context) =>
            {
                if (context.Scale == StatType.MND)
                {
                    context.Power += Power;
                }
            }
        });
    }
}
