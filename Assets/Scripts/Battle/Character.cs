using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class Character
{
    public string Name;
    public CharacterStats Stats;
    public List<Status> Statuses => StatusSystem.Statuses;
    public Ring[] Rings = new Ring[10];
    public List<Func<Damage, Damage>> DealDamageModifiers =>
        Rings.Where(ring => ring != null && ring.DealDamageModifier != null)
             .Select(ring => ring.DealDamageModifier)
             .ToList();
    public List<Func<Damage, Damage>> TakeDamageModifiers =>
        Rings.Where(ring => ring != null && ring.TakeDamageModifier != null)
             .Select(ring => ring.TakeDamageModifier)
             .ToList();

    private List<BattleActionSO> battleActions = new();
    public List<BattleActionSO> BattleActions
    {
        get
        {
            var newBattleActions = new List<BattleActionSO>(battleActions);
            foreach (var ring in Rings)
            {
                if (ring == null) continue; // 跳过空戒指
                foreach (var action in ring.GrantedActions)
                {
                    newBattleActions.Add(action);
                }
                
                if (ring.Empowerment != null)
                    ring.Empowerment.Target = newBattleActions.LastOrDefault();
            }
            return newBattleActions;
        }

        set { battleActions = value; }
    }
    public StatusSystem StatusSystem = new();

    public bool IsPlayerSide = false;
    public bool IsPlayerControlled = false;
    public bool IsDead = false;
    public List<Character> Allies;
    public Character TauntedApplier => Statuses.Where(s => s.Id == 4)
                                               .Select(s => s.Applier)
                                               .FirstOrDefault();

    private float actionGauge = 0f;
    public float ActionGauge
    {
        get { return actionGauge; }
        set
        {
            actionGauge = value;
            OnActionGaugeChanged?.Invoke();
        }
    }
    public event Action OnActionGaugeChanged;

    public Character(CharacterSO so)
    {
        Name = so.CharacterName;
        Stats = so.GetStats(); // 运行时副本
        Stats.Owner = this; // 设置 Stats 的所有者为当前 Character 实例
        Stats.OnHpChanged += HpChanged;
        Rings = so.GetRings();

        // Load Attack Action
        BattleActionSO basicAttackAction = Resources.Load<BattleActionSO>("RingRelated/AttackActionSO");
        battleActions.Add(basicAttackAction);
        BattleActionSO basicDefendAction = Resources.Load<BattleActionSO>("RingRelated/DefendActionSO");
        battleActions.Add(basicDefendAction);

        Stats.InitBeforeBattle();
        StatusSystem.Owner = this; // 设置 StatusSystem 的所有者为当前 Character 实例
    }

    public Character(Character original, float statModifier = 1f, RingInheritType ringInheritType = RingInheritType.None)
    {
        Name = original.Name;
        // 深拷贝 Stats，Owner 指向新角色
        Stats = new(original.Stats, statModifier)
        {
            Owner = this
        };
        Stats.OnHpChanged += HpChanged;

        // 初始化Rings为长度10的数组
        Rings = new Ring[10];
        if (ringInheritType == RingInheritType.All)
        {
            for (int i = 0; i < 10; i++)
                Rings[i] = original.Rings[i];
        }
        else if (ringInheritType == RingInheritType.LeftHand)
        {
            for (int i = 0; i < 10; i += 2)
                Rings[i] = original.Rings[i];
            // 其他位置为null
        }
        else if (ringInheritType == RingInheritType.RightHand)
        {
            for (int i = 1; i < 10; i += 2)
                Rings[i] = original.Rings[i];
            // 其他位置为null
        }
        // None: 全部为空，已初始化

        // 新建 StatusSystem，复制已有状态
        StatusSystem = new StatusSystem
        {
            Owner = this
        };
        foreach (var status in original.StatusSystem.Statuses)
        {
            StatusSystem.AddStatus(status);
        }

        IsPlayerSide = original.IsPlayerSide;
        IsPlayerControlled = original.IsPlayerControlled;
        //IsDead = original.IsDead;
        ActionGauge = original.ActionGauge;
        Stats.InitBeforeBattle();
    }

    public int GetStat(StatType type)
    {
        return Stats.GetStat(type);
    }

    private void HpChanged()
    {
        if (Stats.GetStat(StatType.HP) <= 0)
        {
            IsDead = true;
            BattleAction ba = new()
            {
                Actor = this
            };
            Trigger(TriggerType.OnAfterDeath, ba);
            BattleLoader.I.DestroyCharacter(this);
        }
    }

    public void Trigger(TriggerType type, BattleAction context)
    {
        // 动态收集所有来源的 TriggerEffect
        //List<Action<BattleAction>> effects = new();

        // 戒指的触发器
        foreach (var ring in Rings)
        {
            if (ring == null) continue; // 跳过空戒指
            foreach (var effect in ring.TriggerEffects)
            {
                if (effect.Trigger == type && effect.Effect != null)
                    //effects.Add(effect.Effect);
                    effect.Effect(context);
            }
        }

        // 统一执行
        //foreach (var fx in effects)
        //{
        //    fx(context);
        //}
    }

    public void UpdateDynamicStatMods(StatType statType)    // for beginner's luck & chased status
    {
        foreach (var ring in Rings)
        {
            if (ring == null) continue;
            foreach (var dynamicStatMod in ring.DynamicStatMods)
            {
                if (dynamicStatMod.CheckStatType == statType)
                {
                    dynamicStatMod.Updator(this);
                }
            }
        }

        foreach (var status in StatusSystem.Statuses)
        {
            foreach (var dynamicStatMod in status.DynamicStatMods)
            {
                if (dynamicStatMod.CheckStatType == statType)
                {
                    dynamicStatMod.Updator();
                }
            }
        }
    }

    public void OnWorldTurn()
    {
        StatusSystem.OnTurn(StatusDecayTrigger.OnWorldTurn);
        Stats.ChangeStat(StatType.HP, Stats.GetStat(StatType.HPR));
        Stats.ChangeStat(StatType.MP, Stats.GetStat(StatType.MPR));
    }

    public void OnCharacterTurnBegin()
    {
        StatusSystem.OnTurn(StatusDecayTrigger.OnCharacterTurnBegin);
    }

    public void OnCharacterTurnEnd()
    {
        StatusSystem.OnTurn(StatusDecayTrigger.OnCharacterTurnEnd);
    }
}

public enum RingInheritType
{
    None,
    All,
    LeftHand,
    RightHand,
}