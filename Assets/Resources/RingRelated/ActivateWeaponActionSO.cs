using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/ActivateWeaponActionSO")]
public class ActivateWeaponActionSO : BattleActionSO
{
    //public Action<BattleAction> ArmWeapon;
    public WeaponRingSO WeaponRingSO;
    public List<BattleActionSO> GrantedActions = new();

    public override BattleAction GetAction(Character actor, Character target)
    {
        BattleAction action = base.GetAction(actor, target);
        action.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnAfterResolve,
            Effect = (ba) =>
            {
                ba.Actor.ActivatedWeaponActionSO = this;
            }
        });
        return action;
    }
}
