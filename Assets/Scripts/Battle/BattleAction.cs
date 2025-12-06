using System;
using System.Collections.Generic;
using System.Linq;

public class BattleAction   // it's BattleActionSO + actor and targets
{
    public string Name;
    public Character Actor;
    public Character Target;
    public Damage Damage;
    public List<Status> Statuses = new();
    public bool IsRemovingStatuses = false; // remove statuses listed above
    public Field Field;
    public StatType Scale;
    public int Power = 100; // multiply with dmg/status power
    public int RecoveryTime;
    public RangeType Range;
    public AreaType Area;
    public Element Element;
    public int ManaCost;

    public bool HasExtraLog = false;
    public BattleAction RelatedAction = null;
    public bool IsDoRelatedActionInstead = false;
    public bool IsMissed = false;
    public bool IsCrit = false;
    public bool IsBlocked = false;
    public StatModifier ActorTempStatMod;
    public bool IsDamage = true;
    public bool IsMustSelf = false; // for Must==Self
    public BattleAction FollowUpAction;
    public bool IsFollowUp = false;
    public List<Empowerment> Empowerments = new(); // for action

    //public float scaledValue; // store the calculated damage/status value after scaling

    // 新增：行动后回调（用于像吞噬这种结算后判断的效果）
    public List<TriggerEffect> TriggerEffects = new();

    public void Trigger(TriggerType triggerType)    // eg. for update shield value
    {
        foreach (var te in TriggerEffects)
        {
            if (te.Trigger == triggerType)
            {
                te.Effect?.Invoke(this);
            }
        }
    }

    public void AddFollowUpAction(BattleAction action)
    {
        if (FollowUpAction == null)
        {
            FollowUpAction = action;
            action.IsFollowUp = true;
        }
        else
        {
            FollowUpAction.AddFollowUpAction(action);
        }
    }

    public void ApplyEmpowerment()
    {
        foreach (var emp in Empowerments)
        {
            emp.Effect?.Invoke(this);
        }
    }

    private void ApplyEffectRecursive(Action<BattleAction> effect, BattleAction action)
    {
        effect?.Invoke(action);
        if (action.FollowUpAction != null)
        {
            ApplyEffectRecursive(effect, action.FollowUpAction);
        }
    }

    public BattleAction(BattleAction other)
    {
        Name = other.Name;
        Actor = other.Actor;
        Target = other.Target;
        Damage = other.Damage != null ? new Damage(other.Damage) : null;
        Statuses = other.Statuses.Select(s => new Status(s)).ToList();
        IsRemovingStatuses = other.IsRemovingStatuses;
        Field = other.Field; // 如需深拷贝可自行实现
        Scale = other.Scale;
        Power = other.Power;
        RecoveryTime = other.RecoveryTime;
        Range = other.Range;
        Area = other.Area;
        ManaCost = other.ManaCost;
        HasExtraLog = other.HasExtraLog;
        RelatedAction = other.RelatedAction; // 如需深拷贝可自行实现
        IsDoRelatedActionInstead = other.IsDoRelatedActionInstead;
        IsMissed = other.IsMissed;
        IsCrit = other.IsCrit;
        IsBlocked = other.IsBlocked;
        ActorTempStatMod = other.ActorTempStatMod != null ? new StatModifier(other.ActorTempStatMod) : null;
        IsDamage = other.IsDamage;
        IsMustSelf = other.IsMustSelf;
        IsFollowUp = other.IsFollowUp;
        Empowerments = other.Empowerments.Select(e => e).ToList(); // 如 Empowerment 有拷贝构造可用 new Empowerment(e)
        TriggerEffects = other.TriggerEffects
            .Select(te => new TriggerEffect(te))
            .ToList();

        FollowUpAction = other.FollowUpAction != null ? new BattleAction(other.FollowUpAction) : null;
    }

    public BattleAction() { }
}
