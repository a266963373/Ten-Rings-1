using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/RingSO")]
public class RingSO : ScriptableObject
{
    public int Id;
    public string Name;
    public int Power;    // for son rings' effect strength


    public Ring GetRing()
    {
        Ring ring = new()
        {
            Id = Id,
            Name = Name,
            Power = Power,
        };
        InitRing(ring);

        return ring;
    }

    //protected virtual void SetStatModifiers(List<StatModifier> StatModifiers) { }
    //protected virtual void SetTriggerEffects(List<TriggerEffect> TriggerEffects) { }
    //protected virtual void SetGrantedActions(List<BattleActionSO> GrantedActions) { }
    //protected virtual void SetDealDamageModifier(Func<Damage, Damage> DealDamageModifier) { }
    //protected virtual void SetTakeDamageModifier(Func<Damage, Damage> TakeDamageModifier) { }
    //public virtual void UpdateContext(Character character) { }

    protected virtual void InitRing(Ring ring) { }
}
