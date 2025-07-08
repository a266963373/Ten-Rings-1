using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ThornRingSO")]
public class ThornRingSO : RingSO
{
    protected override void InitTriggerEffects()
    {
        TriggerEffect trigFx = new(
            trigger: TriggerType.OnTakeDamage,
            effect: Effect
            );
        TriggerEffects.Add(trigFx);
    }

    private void Effect(BattleAction context)
    {
        Character attacker = context.Actor;
        Damage damage = new(
            value: power,
            scale: StatType.NON,
            element: DamageElement.Grass
            );
        BattleAction inflictDamageAction = new(
            name: "Inflict Damage",
            actor: null,
            target: attacker,
            damage: damage
            );
        inflictDamageAction.Resolve();
    }
}
