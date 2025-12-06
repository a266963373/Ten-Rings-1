using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "BattleActions/UniversalPotionActionSO")]
public class UniversalPotionActionSO : BattleActionSO 
{
    public override BattleAction GetAction(Character actor, Character target)
    {
        BattleAction baseAction = base.GetAction(actor, target);

        var debuff = target.Statuses.FirstOrDefault(s => !s.IsBuff && s.IsStackable);
        if (debuff != null)
        {
            var removingDebuff = new Status(debuff)
            {
                Applier = actor,
                Stack = StatusStacks[0],
            };
            baseAction.Statuses.Add(removingDebuff);
            baseAction.IsDamage = false;
        }
        else if (target.Stats.HpPercent > target.Stats.MpPercent)
        {
            baseAction.Damage.Property = StatType.MP;
        }
        // else do nothing, heal HP by default

        return baseAction;
    }
}
