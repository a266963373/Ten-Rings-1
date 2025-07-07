using UnityEngine;

public enum BattleActionTargetType
{
    Single,
    Area
}

[CreateAssetMenu(menuName = "BattleAction/BattleActionSO")]
public class BattleActionSO : ScriptableObject
{
    [SerializeField] string actionName;
    [SerializeField] Damage damage;
    [SerializeField] BattleActionTargetType targetType;

    public BattleAction GetAction(Character actor, Character target)
    {
        Damage runtimeDamage = new Damage(damage); // Éî¿½±´
        return new BattleAction(actionName, actor, target, runtimeDamage, targetType);
    }
}
