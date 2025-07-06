using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleActionTargetType
{
    Single,
    Area
}

[CreateAssetMenu(menuName = "BattleAction/BattleActionSO")]
public class BattleActionSO : ScriptableObject
{
    [SerializeField] Damage damage;
    [SerializeField] BattleActionTargetType targetType;

    public BattleAction GetAction(Character actor, Character target)
    {
        Damage runtimeDamage = new Damage(damage); // Éî¿½±´
        return new BattleAction(actor, target, runtimeDamage, targetType);
    }
}

public class BattleAction   // it's BattleActionSO + actor, targets
{
    public Character actor;
    public Character target;
    public Damage damage;
    public BattleActionTargetType targetType;

    public BattleAction(Character actor, Character target, Damage damage, BattleActionTargetType targetType)
    {
        this.actor = actor;
        this.target = target;
        this.damage = damage;
        this.targetType = targetType;
    }
}

[CreateAssetMenu(menuName = "BattleAction/AttackActionSO")]
public class AttackActionSO : BattleActionSO {}
