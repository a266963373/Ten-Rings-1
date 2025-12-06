using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/EnchantmentRingSO")]
public class EnchantmentRingSO : RingSO
{
    public Action<BattleAction> Effect;
    protected override void InitRing(Ring ring)
    {
        ring.Type = RingType.Enchantment;
        ring.Enchantment = new()
        {
            Empowerment = new()
            {
                Effect = Effect
            }
        };
    }
}
