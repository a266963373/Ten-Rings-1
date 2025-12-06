using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Rings/BacklineRingSO")]
public class BacklineRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeTargeted,
            Effect = (BattleAction ba) =>
            {
                if (ba.Actor != null &&
                ba.Actor != ba.Target &&
                ba.Range != RangeType.Indirect &&
                ba.Range != RangeType.Self &&
                ba.Actor.IsPlayerSide != ba.Target.IsPlayerSide &&
                ba.Target.Teammates.Any(c => c.Rings.Any
                (r => r?.Id == 22)))
                {
                    ba.Power = ba.Power * (100 - Power) / 100;
                }
            }
        });
    }
}
