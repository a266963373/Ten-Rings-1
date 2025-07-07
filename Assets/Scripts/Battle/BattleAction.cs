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

    public BattleAction(string name, Character actor, Character target, Damage damage, BattleActionTargetType targetType)
    {
        Name = name;
        Actor = actor;
        Target = target;
        Damage = damage;
        TargetType = targetType;
    }
}
