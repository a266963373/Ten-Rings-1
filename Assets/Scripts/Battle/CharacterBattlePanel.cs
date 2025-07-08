using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class CharacterBattlePanel : MonoBehaviour
{
    [SerializeField] BarController HpBar;
    [SerializeField] BarController MpBar;
    [SerializeField] BarController SpeedBar;
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    private ActionResolver actionResolver;

    private Character character;
    public Character Character
    {
        get { return character; }
        set
        {
            character = value;
            localizeStringEvent.StringReference.SetReference("Character Name", character.Name);
            
            Subscribe();
        }
    }

    public void Initialize(ActionResolver r)
    {
        actionResolver = r;
        UpdateHpBar();
        UpdateMpBar();
        UpdateActionGauge();
    }

    public void UpdateHpBar()
    {
        HpBar.LeftNum = character.Stats.GetStat(StatType.HP);
        HpBar.RightNum = character.Stats.GetStat(StatType.MHP);
        HpBar.LazyUpdate();
    }

    public void UpdateMpBar()
    {
        MpBar.LeftNum = character.Stats.GetStat(StatType.MP);
        MpBar.RightNum = character.Stats.GetStat(StatType.MMP);
        MpBar.LazyUpdate();
    }

    public void UpdateActionGauge()
    {
        SpeedBar.LeftNum = character.ActionGauge;
        SpeedBar.LazyUpdate();
    }

    public void Subscribe() // need to be after BattleSystem.LoadBattle()
    {
        character.Stats.OnHpChanged += UpdateHpBar;
        character.Stats.OnMpChanged += UpdateMpBar;
        character.OnActionGaugeChanged += UpdateActionGauge;
    }

    public void OnClick()
    {
        actionResolver.OnPanelClick(character);
    }
}
