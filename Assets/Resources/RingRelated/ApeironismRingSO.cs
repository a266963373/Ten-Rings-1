using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ApeironismRingSO")]
public class ApeironismRingSO : ProtocolRingSO
{
    protected override void InitRing(Ring ring)
    {
        base.InitRing(ring);    // to set the ring type to Protocol

        ring.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnBeforeDealDamage,
            Effect = (BattleAction ba) =>
            {
                ba.Damage.Element = Element.None;
            },
        });

        ring.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnBeforeTakeDamage,
            Effect = (BattleAction ba) =>
            {
                ba.Damage.Element = Element.None;
            },
        });
    }
}
