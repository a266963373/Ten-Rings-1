using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction   // it's BattleActionSO + actor and targets
{
    public string Name;
    public Character Actor;
    public Character Target;
    public Damage Damage;
    public BattleActionTargetType TargetType;
    public int ManaCost;

    public BattleAction(
        string name, 
        Character actor, 
        Character target, 
        Damage damage, 
        BattleActionTargetType targetType = BattleActionTargetType.Single, 
        int manaCost = 0)
    {
        Name = name;
        Actor = actor;
        Target = target;
        Damage = damage;
        TargetType = targetType;
        ManaCost = manaCost;
    }

    public void Resolve()
    {
        // actually do the action

        if (ManaCost != 0)
        {
            StatModifier manaCostMod = new(
                statType: StatType.MP,
                isPermanent: true,
                value: -ManaCost
                );
            Actor.Stats.AddModifier(manaCostMod);
        }

        int scale = Actor != null ? Actor.Stats.GetStat(Damage.Scale) : 100;
        float scaledValue = Damage.Value * scale * 0.01f;
        Damage.Value = Mathf.RoundToInt(scaledValue);
        StatModifier mod = new(
            isPermanent: true,
            value: -Damage.Value
            );
        Target.Stats.AddModifier(mod);

        Target.Trigger(TriggerType.OnTakeDamage, this);
    }
}
