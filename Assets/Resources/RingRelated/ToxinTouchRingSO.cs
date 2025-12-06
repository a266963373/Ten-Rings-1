using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ToxinTouchRingSO")]
public class ToxinTouchRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        TriggerEffect trigFx = new()
        {
            Trigger = TriggerType.OnAfterDealDamage,
            Effect = Effect
        };
        ring.TriggerEffects.Add(trigFx);
    }

    private void Effect(BattleAction context)
    {
        Character target = context.Target;
        if (target == null) return;

        // 检查是否造成了Bio伤害
        if (context.Damage.Element == Element.Bio)
        {
            ActionResolver.I.ApplyStatusByName(
                context.Actor, target, "Poisoned", Power);
        }
    }
}
