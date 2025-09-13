using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Rings/BacklineRingSO")]
public class BacklineRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeTakeDamage,
            Effect = (BattleAction ba) =>
            {
                if (ba.Target.Allies.Any(c => c != ba.Target && c.Rings.Any
                (r => r?.Id == 22)))
                {
                    ring.TakeDamageModifier = Effect;
                } else
                {
                    ring.TakeDamageModifier = null;
                }
            }
        });
    }

    private Damage Effect(Damage dmg)
    {
        if (dmg.Range != DamageRange.Indirect)
            dmg.Value = dmg.Value * (100 - Power) / 100;
        return dmg;
    }
}
