using System;
using System.Collections.Generic;

public class Ring
{
    public int Id;
    public string Name;
    public int Power;
    public int Count = 0;   // for some rings like hammer tempered

    public List<StatModifier> StatModifiers = new();
    public List<TriggerEffect> TriggerEffects = new();
    public List<BattleActionSO> GrantedActions = new();
    public Func<Damage, Damage> DealDamageModifier;
    public Func<Damage, Damage> TakeDamageModifier;
    public List<RingDynamicStatMod> DynamicStatMods = new();
    public Empowerment Empowerment;
}
