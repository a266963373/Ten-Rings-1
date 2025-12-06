using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/WeaponRingSO")]
public class WeaponRingSO : RingSO
{
    [SerializeField] List<BattleActionSO> GrantedActions = new();

    protected override void InitRing(Ring ring)
    {
        ring.Type = RingType.Weapon;
        ring.Element = Element.Metal;

        ActivateWeaponActionSO activateWeaponActionSO = Instantiate(BattleActionLibrary.I.ActivateWeaponActionSO);
        activateWeaponActionSO.WeaponRingSO = this;
        activateWeaponActionSO.GrantedActions = GrantedActions;
        ring.GrantedActions.Add(activateWeaponActionSO);
    }
}

public class Enchantment    // only for weapon ring
{
    public Empowerment Empowerment = new();
}
