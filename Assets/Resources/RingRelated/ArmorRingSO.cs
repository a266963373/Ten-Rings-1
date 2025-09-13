using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ArmorRingSO")]
public class ArmorRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TakeDamageModifier = (Damage dmg) =>
        {
            if (dmg.Form == DamageForm.Blunt)
            {
                dmg.Reduction += Power;
            }
            else if (dmg.Form == DamageForm.Sharp)
            {
                dmg.Reduction += 2 * Power;
            }
            return dmg;
        };
    }
}
