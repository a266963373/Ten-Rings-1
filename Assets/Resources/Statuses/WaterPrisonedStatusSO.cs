using UnityEngine;

[CreateAssetMenu(menuName = "Status/WaterPrisonedStatusSO")]
public class WaterPrisonedStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.StatModifiers.Add(new StatModifier
        {
            StatType = StatType.SPD,
            Value = -Power
        });

        status.TriggerEffects.Add(new TriggerEffect()
        {
            Timing = TimingType.OnWorldTurn,
            EffectOnTime = () =>
            {
                BattleAction ba = new BattleAction
                {
                    Name = status.Name,
                    Target = status.Bearer,
                    Range = RangeType.Indirect,
                    Element = Element.Water,
                    Damage = new Damage
                    {
                        Value = Power2,
                        Element = Element.Water,
                        Form = DamageForm.Internal,
                    },
                    IsDamage = true,
                };

                ActionResolver.I.Resolve(ba);
            }
        });
    }
}
