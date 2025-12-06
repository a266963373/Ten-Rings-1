using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/ChantActionSO")]
public class ChantActionSO : BattleActionSO
{
    public override BattleAction GetAction(Character actor, Character target)
    {
        BattleAction action = base.GetAction(actor, target);
        action.ManaCost = Mathf.RoundToInt(action.ManaCost *
            actor.Stats.GetStat(StatType.MMP) / 100f);
        return action;
    }
}
