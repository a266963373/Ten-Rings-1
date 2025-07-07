using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ActionResolver : MonoBehaviour
{
    [NonSerialized] public bool IsTargetSelectMode = false;
    [NonSerialized] public BattleActionSO BattleActionSO;
    [SerializeField] BattleLogSystem battleLogSystem;
    private BattleAction battleAction;
    public Character Actor;
    public Character Target;

    public void OnPanelClick(Character target)
    {
        if (IsTargetSelectMode)
        {
            Target = target;
            StartResolve();
            IsTargetSelectMode = false ;
        }
    }

    public void StartResolve()
    {
        battleAction = BattleActionSO.GetAction(Actor, Target);
        battleLogSystem.ShowBattleAction(battleAction, ProcessBattleAction, true);
    }

    private void ProcessBattleAction()
    {
        Attack();
        // Battle Action Processed
        battleLogSystem.ShowActionResult(battleAction, ActionResolved, true);
    }

    private void ActionResolved()
    {
        BattleSystem.I.State = BattleState.Idle;
    }

    private void Attack()
    {
        Debug.Log(battleAction.Damage.Value);
        //battleAction.damage.Value = (int)(battleAction.damage.Value * Actor.Stats.GetStat(battleAction.damage.Scale) * 0.01f);
        float scaled = battleAction.Damage.Value * Actor.Stats.GetStat(battleAction.Damage.Scale) * 0.01f;
        battleAction.Damage.Value = Mathf.RoundToInt(scaled);
        StatModifier mod = new()
        {
            IsPermanent = true,
            Value = -battleAction.Damage.Value,
        };
        Target.Stats.AddModifier(mod);
    }
}
