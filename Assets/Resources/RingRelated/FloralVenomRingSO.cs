using UnityEngine;

[CreateAssetMenu(menuName = "Rings/FloralVenomRingSO")]
public class FloralVenomRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        TriggerEffect trigFx = new()
        {
            Trigger = TriggerType.OnAfterDealDamage,
            Effect = (ba) =>
            {
                if (ba.IsDamage && ba.Damage.Element == Element.Grass)
                {
                    ActionResolver.I.ApplyStatusByName(
                        ba.Actor, ba.Target, "Poisoned", Power);
                }
            }
        };
        ring.TriggerEffects.Add(trigFx);
    }
}
