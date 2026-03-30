using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/SeedTheoryRingSO")]
public class SeedTheoryRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnBeforeDeath,
            Effect = (BattleAction ba) =>
            {
                if (ba.Actor.GetStat(StatType.MHP) > 50)
                {
                    ba.Actor.Stats.ChangeStat(StatType.MHP, 0.5f, ModifierType.Percent);
                    ba.Actor.Stats.ChangeStat(StatType.HP, 1, ModifierType.Set);
                    ba.Actor.IsDeathDefied = true;
                }
            },
        });
    }
}
