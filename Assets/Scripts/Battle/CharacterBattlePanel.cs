using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattlePanel : MonoBehaviour
{
    [SerializeField] BarController HpBar;
    [SerializeField] BarController SpeedBar;
    private ActionResolver actionResolver;

    private Character character;
    public Character Character
    {
        get { return character; }
        set
        {
            character = value;
            Subscribe();
        }
    }

    public void Initialize(ActionResolver r)
    {
        actionResolver = r;
        UpdateHpBar();
        UpdateActionGauge();
    }

    public void UpdateHpBar()
    {
        HpBar.LeftNum = character.Stats.GetStat(StatType.HP);
        HpBar.RightNum = character.Stats.GetStat(StatType.MHP);
        HpBar.LazyUpdate();
    }

    public void UpdateActionGauge()
    {
        SpeedBar.LeftNum = character.ActionGauge;
        SpeedBar.LazyUpdate();
    }

    public void Subscribe() // need to be after BattleSystem.LoadBattle()
    {
        character.Stats.OnHpChanged += UpdateHpBar;
        character.OnActionGaugeChanged += UpdateActionGauge;
    }

    public void OnClick()
    {
        actionResolver.OnPanelClick(character);
    }
}
