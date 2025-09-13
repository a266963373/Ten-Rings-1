using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/SkillRingSO")]
public class SkillRingSO : RingSO
{
    [SerializeField] List<BattleActionSO> GrantedActions = new();

    protected override void InitRing(Ring ring)
    {
        ring.GrantedActions.AddRange(GrantedActions);
    }
}
// just put action SO into the GrantedActionList.
