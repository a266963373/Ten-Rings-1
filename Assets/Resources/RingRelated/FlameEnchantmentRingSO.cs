using UnityEngine;

[CreateAssetMenu(menuName = "Rings/FlameEnchantmentRingSO")]
public class FlameEnchantmentRingSO : EnchantmentRingSO
{
    protected override void InitEffect(Ring ring)
    {
        Effect = (ba) =>
        {
            ba.TriggerEffects.Add(new TriggerEffect
                {
                    Trigger = TriggerType.OnAfterDealDamage,
                    Effect = (action) =>
                    { 
                        BattleAction extraDmgAction = new()
                        {
                            Name = "Enchanted Damage",
                            Range = ba.Range,
                            Area = ba.Area,
                            Target = action.Target,
                            Damage = new()
                            {
                                Element = Element.Fire,
                                Value = action.Damage.Value * Power / 100,
                                Form = DamageForm.Thermal,
                            },
                            IsDamage = true,
                            IsFollowUp = true,
                        };
                        ActionResolver.I.Resolve(extraDmgAction);
                    }
                });
        };
    }
}
