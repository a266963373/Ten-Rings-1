using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/HylomorphismRingSO")]
public class HylomorphismRingSO : ProtocolRingSO
{
    protected override void InitRing(Ring ring)
    {
        base.InitRing(ring);    // to set the ring type to Protocol

        ring.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnAfterMeleeAction,
            Effect = (BattleAction ba) =>
            {
                if (ba.Target.IsPlayerSide == ba.Actor.IsPlayerSide
                    && ba.Target != ba.Actor)
                {
                    var HpAverage = (ba.Actor.GetStat(StatType.HP) 
                        + ba.Target.GetStat(StatType.HP)) / 2;
                    ba.Actor.Stats.ChangeStat(StatType.HP, HpAverage);
                    ba.Target.Stats.ChangeStat(StatType.HP, HpAverage);
                }
            },
        });
    }
}
