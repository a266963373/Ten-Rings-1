using System;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name;
    public CharacterStats Stats;
    public List<RingSO> Rings = new();
    public List<BattleActionSO> BattleActions = new();
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
        BattleActionSO newBattleAction = Resources.Load<BattleActionSO>("RingRelated/AttackActionSO");
        BattleActions.Add(newBattleAction);

        Stats.InitBeforeBattle();
        StatusSystem.Owner = this; // 设置 StatusSystem 的所有者为当前 Character 实例
    }

    private void HpChanged()
    {
        if (Stats.GetStat(StatType.HP) <= 0)
        {
            IsDead = true;
        }
    }

    public void Trigger(TriggerType type, BattleAction context)
    {
        // 动态收集所有来源的 TriggerEffect
        List<Action<BattleAction>> effects = new();

        // 戒指的触发器
        foreach (var ring in Rings)
        {
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
