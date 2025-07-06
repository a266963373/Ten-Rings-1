using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ActionResolver : MonoBehaviour
{
    [NonSerialized] public bool IsTargetSelectMode = false;
    [NonSerialized] public BattleActionSO BattleActionSO;
    private BattleAction battleAction;
    public Character Actor;
    public Character Target;

    public void OnPanelClick(Character target)
    {
        Debug.Log("Panel Clicked");

        if (IsTargetSelectMode)
        {
            Target = target;
            StartResolve();
        }
    }

    public void StartResolve()
    {
        battleAction = BattleActionSO.GetAction(Actor, Target);
        Attack();
    }

    private void Attack()
    {
        Debug.Log(battleAction.damage.Value);
        //battleAction.damage.Value = (int)(battleAction.damage.Value * Actor.Stats.GetStat(battleAction.damage.Scale) * 0.01f);
        float scaled = battleAction.damage.Value * Actor.Stats.GetStat(battleAction.damage.Scale) * 0.01f;
        battleAction.damage.Value = Mathf.RoundToInt(scaled);
        StatModifier mod = new()
        {
            IsPermanent = true,
            Value = -battleAction.damage.Value,
        };
        Target.Stats.AddModifier(mod);
    }
}
