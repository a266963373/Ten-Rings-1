using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ThornRingSO")]
public class ThornRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        TriggerEffect trigFx = new()
        {
            Trigger = TriggerType.OnAfterTakeDamage,
            Effect = Effect
        };
        ring.TriggerEffects.Add(trigFx);
    }

    private void Effect(BattleAction context)
    {
        Character attacker = context.Actor;
        if (attacker == null || context.Range != RangeType.Melee) return;
        Damage damage = new()
        {
            Value = Power,
            Form = DamageForm.Sharp,
            Element = Element.Grass
        };
        BattleAction inflictDamageAction = new()
        {
            Name = "Thorn",
            Actor = null,
            Target = attacker,
            Damage = damage,
        };
        ActionResolver.I.Resolve(inflictDamageAction, true);
    }
}
