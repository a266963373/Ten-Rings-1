using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/OmniLectureActionSO")]
public class OmniLectureActionSO : BattleActionSO 
{
    public override BattleAction GetAction(Character actor, Character target)
    {
        BattleAction baseAction = base.GetAction(actor, target);
        baseAction.HasExtraLog = true;
        baseAction.RelatedAction = RingLibrary.I.GetRandomRings(1, idBlacklist: new(){4}, isSkillRing: true)
            [0].GrantedActions[0].GetAction(actor, target);
        baseAction.RelatedAction.ManaCost = baseAction.ManaCost;
        baseAction.IsDoRelatedActionInstead = true;
        return baseAction;
    }

}
