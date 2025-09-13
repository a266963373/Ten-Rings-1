using UnityEngine;

[CreateAssetMenu(menuName = "Rings/HammerTemperedRingSO")]
public class HammerTemperedRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.Empowerment = new Empowerment
        {
            Effect = (action) =>
            {
                action.TriggerEffects.Add(new TriggerEffect
                {
                    Trigger = TriggerType.OnAfterResolve,
                    Effect = (ba) =>
                    {
                        ring.Count++;
                    }
                });

                action.TriggerEffects.Add(new TriggerEffect
                {
                    Trigger = TriggerType.OnBeforeAction,
                    Effect = (ba) =>
                    {
                        ba.Power += ring.Count * Power;
                    }
                });
            }
        };
    }
}
