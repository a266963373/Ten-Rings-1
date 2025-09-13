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
    Killable,
    WithoutMyStatus,
}

[CreateAssetMenu(menuName = "BattleActions/BattleActionSO")]
public class BattleActionSO : ScriptableObject
{
    public string Name;
    public Damage Damage;
    public StatusSO Status;
    public int Power = 100;
    public int ManaCost;
    public AreaType Area;
    public TargetType Must;     // must target, if self then no selection
    public TargetType Prefer;   // prefer to target (used for npc)
    public TargetType Favorate;   // prioritize target (used for npc)
    public bool IsDamage = true; // false means buff/heal
    public bool IsStatus = false;
    public StatType Scale;

    public virtual BattleAction GetAction(Character actor, Character target)
    {
        Damage runtimeDamage = new(Damage); // Éî¿½±´
        Status runtimeStatus = null;
        if (IsStatus)
        {
            runtimeStatus = Status.GetStatus(actor, target);
        }

        BattleAction action = new()
        {
            Name = Name,
            Actor = actor, 
            Target = target, 
            Power = Power,
            Damage = runtimeDamage,
            Status = runtimeStatus,
            Area = Area,
            ManaCost = ManaCost,
            IsDamage = IsDamage,
            IsStatus = IsStatus,
            Scale = Scale,
        };


        // empowerment
        foreach (Ring ring in actor.Rings)
        {
            if (ring != null && ring.Empowerment != null && ring.Empowerment.Target == this)
                ring.Empowerment.Effect(action);
        }

        return action;
    }
}
