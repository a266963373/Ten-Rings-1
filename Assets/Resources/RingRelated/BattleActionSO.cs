using UnityEngine;

public enum BattleActionTargetType
{
    Single,
    Area
}

[CreateAssetMenu(menuName = "BattleAction/BattleActionSO")]
public class BattleActionSO : ScriptableObject
{
    public string Name;
    public Damage Damage;
    public BattleActionTargetType TargetType;
    public int ManaCost;

    public BattleAction GetAction(Character actor, Character target)
    {
        Damage runtimeDamage = new Damage(Damage); // Éî¿½±´
        return new BattleAction(Name, actor, target, runtimeDamage, TargetType, ManaCost);
    }
}
