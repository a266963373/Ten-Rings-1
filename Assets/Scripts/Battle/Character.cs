using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character
{
    public string Name;
    public CharacterStats Stats;
    public RingSO[] Rings = new RingSO[10];
    //public List<RingSO> LeftHandRings => Rings
    //    .Where((r, i) => i % 2 == 0)
    //    .ToList();
    //public List<RingSO> RightHandRings => Rings
    //    .Where((r, i) => i % 2 != 0)
    //    .ToList();

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
            }
            return newBattleActions;
        }

        set { battleActions = value; }
    }
    public StatusSystem StatusSystem = new();

    public bool IsPlayerSide = false;
    public bool IsPlayerControlled = false;
    public bool IsDead = false;

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
        Rings = new RingSO[10];
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

        // 深拷贝 BattleActions（引用 ScriptableObject，通常可直接赋值）
        BattleActions = new List<BattleActionSO>(original.BattleActions);

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
        List<Action<BattleAction>> effects = new();

        // 戒指的触发器
        foreach (var ring in Rings)
        {
            if (ring == null) continue; // 跳过空戒指
            foreach (var effect in ring.TriggerEffects)
            {
                if (effect.Trigger == type && effect.Effect != null)
                    effects.Add(effect.Effect);
            }
        }

        // 统一执行
        foreach (var fx in effects)
        {
            fx(context);
        }
    }

    public void OnWorldTurn()
    {
        StatusSystem.OnWorldTurn();
    }
}

public enum RingInheritType
{
    None,
    All,
    LeftHand,
    RightHand,
}