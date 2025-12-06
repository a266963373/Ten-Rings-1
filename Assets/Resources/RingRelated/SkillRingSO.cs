using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/SkillRingSO")]
public class SkillRingSO : RingSO
{
    [SerializeField] List<BattleActionSO> GrantedActions = new();

    // higher level ring needs lower level ring of the same element equipeed
    public int level = 1;
    public Element Element = Element.Fire;

    protected override void InitRing(Ring ring)
    {
        ring.ActionLevel = level;
        ring.Element = Element;
        ring.Type = RingType.Skill;
        foreach (var action in GrantedActions)
        {
            action.Element = Element;
        }
        ring.GrantedActions.AddRange(GrantedActions);
    }
}
// just put action SO into the GrantedActionList.
