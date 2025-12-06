using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Rings/LastBurstRingSO")]
public class LastBurstRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeDeath,
            Effect = (ba) =>
            {
                BattleAction newBa = new()
                {
                    Name = "Last Burst",
                    Actor = ba.Actor,
                    Target = ba.Actor.LastTarget ?? ba.Actor.RandomOther,
                    Range = RangeType.Ranged,
                    Damage = new Damage
                    {
                        Value = Mathf.RoundToInt(Power * ba.Actor.GetStat(StatType.MHP) / 100f),
                        Form = DamageForm.Thermal,
                        Element = Element.Fire,
                    },
                };
                ActionResolver.I.Resolve(newBa);
            }
        });
    }
}
