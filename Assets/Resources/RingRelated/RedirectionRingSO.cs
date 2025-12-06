using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/RedirectionRingSO")]
public class RedirectionRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeAction,
            Effect = (ba) =>
            {
                if (ba.Actor.LastTarget != null && ba.Target != ba.Actor.LastTarget)
                {
                    ba.Target = ba.Actor.LastTarget;
                }
            }
        });
    }
}
