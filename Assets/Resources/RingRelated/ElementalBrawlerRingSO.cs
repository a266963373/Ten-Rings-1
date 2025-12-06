using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ElementalBrawlerRingSO")]
public class ElementalBrawlerRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new()
        {
            Trigger = TriggerType.OnBeforeAction,
            Effect = (BattleAction ba) =>
            {
                if (ba.Damage.Element == Element.Bio)
                {
                    for (int i = 0; i < ba.Actor.Rings.Length; i++)
                    {
                        var ringI = ba.Actor.Rings[i];
                        if (ringI != null) continue;
                        if (ringI == ring) break; // stop at this ring
                        if (ringI.Type == RingType.Form)
                        {
                            ba.Damage.Element = ringI.Element;
                            break;
                        }
                    }
                }
            }
        });
    }
}
