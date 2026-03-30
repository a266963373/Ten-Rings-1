using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/TheFourElementsRingSO")]
public class TheFourElementsRingSO : RingSO
{
    readonly List<Element> familiarElements = new()
    {
        Element.Fire,
        Element.Water,
        Element.Earth,
        Element.Air
    };
    protected override void InitRing(Ring ring)
    {

        ring.DealDamageModifier = Effect;
        ring.TakeDamageModifier = Effect;
    }

    private Damage Effect(Damage dmg)
    {
        if (!familiarElements.Contains(dmg.Element))
        {
            dmg.Value /= Power;
        }
        return dmg;
    }
}
