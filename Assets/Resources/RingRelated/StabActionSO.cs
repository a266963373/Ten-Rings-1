using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/StabActionSO")]
public class StabActionSO : BattleActionSO
{
    public override BattleAction GetAction(Character actor, Character target)
    {
        BattleAction action = base.GetAction(actor, target);
        
        int excessiveAcc = actor.Stats.GetStat(StatType.ACC) - target.Stats.GetStat(StatType.EVS);
        action.ActorTempStatMod = new()
        {
            StatType = StatType.CRT,
            Value = excessiveAcc,
            Source = Name,
        };

        return action;
    }
}
