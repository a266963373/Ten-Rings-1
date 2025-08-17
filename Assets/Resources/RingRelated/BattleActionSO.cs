using UnityEngine;

public enum AreaType
{
    Single,
    Area
}

public enum TargetType
{
    None,
    Self,
    Enemy,
    Ally,
    Killable // ÐÂÔö
}

[CreateAssetMenu(menuName = "BattleActions/BattleActionSO")]
public class BattleActionSO : ScriptableObject
{
    public string Name;
    public Damage Damage;
    public int ManaCost;
    public AreaType Area;
    public TargetType Limit;    // cannot target
    public TargetType Prefer;   // prefer to target (used for npc)
    public TargetType Favorate;   // prioritize target (used for npc)

    public virtual BattleAction GetAction(Character actor, Character target)
    {
        Damage runtimeDamage = new(Damage); // Éî¿½±´
        return new()
        {
            Name = Name,
            Actor = actor, 
            Target = target, 
            Damage = runtimeDamage,
            Area = Area,
            Limit = Limit,
            Prefer = Prefer,
            ManaCost = ManaCost
        };
    }
}
