using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRingSO : SkillRingSO
{
    protected override void InitRing(Ring ring)
    {
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
