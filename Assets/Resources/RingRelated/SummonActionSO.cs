using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/SummonActionSO")]
public class SummonActionSO : BattleActionSO
{
    [SerializeField] CharacterSO summon;
    protected override void InitAction(BattleAction action)
    {
        action.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnExtraEffect,
            Effect = (ba) =>
            {
                BattleLoader.I.LoadCharacterSO(summon, action.Actor.IsPlayerSide);
            }
        });
    }
}
