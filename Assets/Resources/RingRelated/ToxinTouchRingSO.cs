using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ToxinTouchRingSO")]
public class ToxinTouchRingSO : RingSO
{
    protected override void InitTriggerEffects()
    {
        TriggerEffect trigFx = new(
            trigger: TriggerType.OnAfterDealDamage,
            effect: Effect
            );
        TriggerEffects.Add(trigFx);
    }

    private void Effect(BattleAction context)
    {
        Character target = context.Target;
        if (target == null) return;

        // 检查是否造成了Bio伤害
        if (context.Damage.Element == DamageElement.Bio)
        {
            target.StatusSystem.AddStatus(StatusLibrary.I.GetStatusByName("Poisoned"));
        }
    }
}
