using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Character
{
    public string Name;
    public CharacterStats Stats;
    public List<BattleActionSO> BattleActions = new();
    public bool IsPlayerSide = false;
    public bool IsPlayerControlled = false;

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
        BattleActionSO newBattleAction = Resources.Load<BattleActionSO>("ScriptableObjects/BattleActions/AttackActionSO");
        BattleActions.Add(newBattleAction);
    }

    public void StartBattle()
    {
        ActionGauge = 0f;
    }
}
