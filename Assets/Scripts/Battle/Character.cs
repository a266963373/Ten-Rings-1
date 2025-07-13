using System;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name;
    public CharacterStats Stats;
    public List<RingSO> Rings = new();
    public List<BattleActionSO> BattleActions = new();
    public Dictionary<TriggerType, List<Action<BattleAction>>> TriggerEffects = new();
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
        Stats.OnHpChanged += HpChanged;
        Rings = so.GetRings();

        // Load Attack Action
        BattleActionSO newBattleAction = Resources.Load<BattleActionSO>("RingRelated/AttackActionSO");
        BattleActions.Add(newBattleAction);

        // Init Dict
        foreach (TriggerType type in Enum.GetValues(typeof(TriggerType)))
        {
            TriggerEffects[type] = new List<Action<BattleAction>>();
        }

        // Load Rings
        LoadRings();
        Stats.InitBeforeBattle();
    }

    private void HpChanged()
    {
        if (Stats.GetStat(StatType.HP) <= 0)
        {
            IsDead = true;
        }
    }

    public void StartBattle()
    {
        ActionGauge = 0f;
    }

    private void LoadRings()
    {
        foreach (var r in Rings)
        {
            r.AffectCharacter(this);
        }
    }

    public void Trigger(TriggerType type, BattleAction context)
    {
        foreach (var fx in TriggerEffects[type])
        {
            fx(context);
        }
    }

}
