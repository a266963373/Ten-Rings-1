using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/BlazingRayActionSO")]
public class BlazingRayActionSO : BattleActionSO
{
    public int SegmentCount = 5;

    public override BattleAction GetAction(Character actor, Character target)
    {
        BattleAction action = base.GetAction(actor, target);

        for (int i = 0; i < SegmentCount - 2; ++i)
            // -2 because itself has dmg and it has 1 followup already
        {
            action.AddFollowUpAction(FollowUpAction.GetAction(actor, target));
        }

        return action;
    }
}