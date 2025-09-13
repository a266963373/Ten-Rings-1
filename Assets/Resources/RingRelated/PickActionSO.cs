using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/PickActionSO")]
public class PickActionSO : BattleActionSO
{
    public override BattleAction GetAction(Character actor, Character target)
    {
        BattleAction action = base.GetAction(actor, target);
        action.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeDealDamage,
            Effect = (ba) =>
            {
                action.Damage.Shield = action.Damage.Value / 2;
            }
        });

        return action;
    }
}
