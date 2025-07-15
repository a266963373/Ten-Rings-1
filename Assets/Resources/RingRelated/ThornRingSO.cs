using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ThornRingSO")]
public class ThornRingSO : RingSO
{
    protected override void InitTriggerEffects()
    {
        TriggerEffect trigFx = new(
            trigger: TriggerType.OnAfterTakeDamage,
            effect: Effect
            );
        TriggerEffects.Add(trigFx);
    }

    private void Effect(BattleAction context)
    {
        Character attacker = context.Actor;
        if (attacker == null || context.Damage.Range != DamageRange.Melee) return;
        Damage damage = new()
        {
            Value = Power,
            Scale = StatType.NON,
            Range = DamageRange.Indirect,
            Element = DamageElement.Grass
        };
        BattleAction inflictDamageAction = new()
        {
            Name = "Inflict Damage",
            Actor = null,
            Target = attacker,
            Damage = damage,
        };
        inflictDamageAction.Resolve();
    }
}
