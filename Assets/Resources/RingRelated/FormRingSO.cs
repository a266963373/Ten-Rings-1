using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/FormRingSO")]
public class FormRingSO : RingSO
{
    public Element Element;
    [SerializeField] List<Element> resistElements;
    [SerializeField] List<Element> weakElements;

    protected override void InitRing(Ring ring)
    {
        ring.Type = RingType.Form;
        ring.Element = Element;

        ring.TakeDamageModifier = (Damage dmg) =>
        {
            if (Element == dmg.Element)
            {
                dmg.Power -= Power * 2;
            }
            else if (resistElements.Contains(dmg.Element))
            {
                dmg.Power -= Power; // reduce effectiveness
            }
            else if (weakElements.Contains(dmg.Element))
            {
                dmg.Power += Power; // increase effectiveness
            }
            return dmg;
        };
    }
}
