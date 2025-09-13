using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/SlashActionSO")]
public class SlashActionSO : BattleActionSO
{
    public override BattleAction GetAction(Character actor, Character target)
    {
        BattleAction action = base.GetAction(actor, target);
        float scale = (actor.Stats.GetStat(StatType.STR) 
            + actor.Stats.GetStat(StatType.SPD)
            + actor.Stats.GetStat(StatType.CRT)
            + actor.Stats.GetStat(StatType.EVS)) / 4f;
        action.Damage.Value = Mathf.RoundToInt(action.Damage.Value * scale * 0.01f); // scale the damage based on actor's stats

        return action;
    }
}
