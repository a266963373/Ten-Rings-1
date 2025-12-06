using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Rings/RemnantFlameRingSO")]
public class RemnantFlameRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeAction,
            Effect = (ba) =>
            {
                if (ba.Damage.Element == Element.Fire)
                {
                    ba.Damage.Power -= Power;   // 50% less effective
                    float stack = ba.Damage.Value * Power / 1000f;
                    ba.Statuses.Add(StatusLibrary.I.GetStatusByName(ba.Actor, ba.Target, "Burning", stack));
                }
            }
        });
    }
}
