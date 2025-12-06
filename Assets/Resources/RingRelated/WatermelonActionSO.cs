using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "BattleActions/WatermelonActionSO")]
public class WatermelonActionSO : BattleActionSO 
{
    protected override void InitAction(BattleAction action)
    {
        action.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeAction,
            Effect = (BattleAction ba) =>
            {
                if (ba.Actor.IsPlayerSide == ba.Target.IsPlayerSide)
                {
                    ba.Damage.Value *= -1;
                }
            }
        });
    }
}
