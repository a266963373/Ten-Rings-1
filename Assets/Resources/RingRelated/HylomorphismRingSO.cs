using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/HylomorphismRingSO")]
public class HylomorphismRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
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
                    bool isActorHigherHp = ba.Actor.GetStat(StatType.HP) > ba.Target.GetStat(StatType.HP);

                    int finalChange;
                    if (!isActorHigherHp)
                    {
                        finalChange = Math.Min(HpAverage - ba.Actor.GetStat(StatType.HP),
                            ba.Actor.GetStat(StatType.MHP) - ba.Actor.GetStat(StatType.HP));
                    } else
                    {
                        finalChange = Math.Min(HpAverage - ba.Target.GetStat(StatType.HP),
                            ba.Target.GetStat(StatType.MHP) - ba.Target.GetStat(StatType.HP));
                    }

                    int changeFactor = isActorHigherHp ? 1 : -1;
                    ba.Actor.Stats.ChangeStat(StatType.HP, HpAverage * -changeFactor);
                    ba.Target.Stats.ChangeStat(StatType.HP, HpAverage * changeFactor);
                }
            },
        });
    }
}
