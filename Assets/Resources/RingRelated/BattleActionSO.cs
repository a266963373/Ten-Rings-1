using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    Melee,
    Ranged,
    Global,  // for "insult skill"
    Indirect, // for "poison skill"
    Self,
}

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
    Field,
    LowestPercentHpAlly,
}

[CreateAssetMenu(menuName = "BattleActions/BattleActionSO")]
public class BattleActionSO : ScriptableObject
{
    public string Name;
    public Damage Damage;
    public List<StatusSO> Statuses = new();
    public List<int> StatusStacks = new(); // starting stack for status
    public int Power = 100;
    public int RecoveryTime = 100; // after use action, how long until next action
    public int ManaCost;
    public int ManaCostRunTime
    {
        get
        {
            if (this is InvokeActionSO ia && ia.IsActionInvoked)
            {
                return 0;
            }
            return FieldSystem.I.Trigger(this, IntModType.ManaCost);
        }
    }

    public RangeType Range;
    public AreaType Area;
    public TargetType Must;     // must target, if self then no selection
    public TargetType Prefer;   // prefer to target (used for npc)
    public TargetType Favorate;   // prioritize target (used for npc)
    public bool IsDamage = true;    // deal damage?
    public StatType Scale;
    public BattleActionSO FollowUpAction; // execute after this action

    // given by ring
    [NonSerialized] public Element Element;

    public virtual BattleAction GetAction(Character actor, Character target)
    {
        Damage runtimeDamage = new(Damage); // 深拷贝

        BattleAction action = new()
        {
            Name = Name,
            Actor = actor,
            Target = target,
            Power = Power,
            Damage = runtimeDamage,
            RecoveryTime = RecoveryTime,
            Range = Range,
            Area = Area,
            Element = Element,
            ManaCost = ManaCostRunTime,
            IsDamage = IsDamage,
            Scale = Scale,
            IsMustSelf = (Must == TargetType.Self),
        };

        if (FollowUpAction != null)
        {
            action.FollowUpAction = FollowUpAction.GetAction(actor, target);
            action.FollowUpAction.IsFollowUp = true;
        }

        if (action.IsMustSelf)
        {
            action.Target = action.Actor;
        }

        InitActionStatus(action);

        // ring 赋能
        if (actor != null && actor.Rings != null)
        {
            foreach (var ring in actor.Rings.Where(r => r?.Empowerment?.Target == this))
            {
                action.Empowerments.Add(ring.Empowerment);
            }
            foreach (var ring in actor.Rings.Where(r => r?.Enchantment?.Empowerment?.Target == this))
            {
                action.Empowerments.Add(ring.Enchantment.Empowerment);
            }
        }

        InitAction(action);

        return action;
    }

    protected virtual void InitAction(BattleAction action) { }

    public bool ContainsStatus(Status s)
    {
        foreach (var statusSO in Statuses)
        {
            if (statusSO != null && statusSO.Id == s.Id)
            {
                return true;
            }
        }
        return false;
    }

    protected void InitActionStatus(BattleAction action)
    {
        if (Statuses != null && Statuses.Count > 0)
        {
            action.Statuses.Clear();
            for (int i = 0; i < Statuses.Count; i++)
            {
                var so = Statuses[i];
                if (so == null) continue;

                Status runtimeStatus = so.GetStatus(action.Actor, action.Target);

                if (i < StatusStacks.Count)
                {
                    runtimeStatus.Stack = StatusStacks[i];
                }
                // 否则沿用 StatusSO 的 StartStack（已在 GetStatus 内设置）

                action.Statuses.Add(runtimeStatus);
            }
        }
    }
}
