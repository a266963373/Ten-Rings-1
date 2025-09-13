using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Rings/FrontlineRingSO")]
public class FrontlineRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeAction,
            Effect = (BattleAction ba) =>
            {
                if (ba.Damage.Range == DamageRange.Melee)
                {
                    ba.Actor.Stats.TempStatMods.Add(new StatModifier
                    {
                        StatType = StatType.ACC,
                        Value = Power,
                    });
                }
            }
        });
    }
}
