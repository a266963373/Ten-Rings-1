using UnityEngine;

public enum BattleActionTargetType
{
    Single,
    Area
}

[CreateAssetMenu(menuName = "BattleActions/BattleActionSO")]
public class BattleActionSO : ScriptableObject
{
    public string Name;
    public Damage Damage;
    public BattleActionTargetType TargetType;
    public int ManaCost;
    //public BattleAction RelatedAction = null;
    //public bool HasExtraLog = false;

    public virtual BattleAction GetAction(Character actor, Character target)
    {
        Damage runtimeDamage = new(Damage); // Éî¿½±´
        return new()
        {
            Name = Name,
            Actor = actor, 
            Target = target, 
            Damage = runtimeDamage, 
            TargetType = TargetType, 
            ManaCost = ManaCost
        };
    }
}
