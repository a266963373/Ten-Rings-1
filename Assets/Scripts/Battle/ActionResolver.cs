using System;
using UnityEngine;

public class ActionResolver : MonoBehaviour // receive choice from panels
{
    [NonSerialized] public bool IsTargetSelectMode = false;
    [NonSerialized] public BattleActionSO BattleActionSO;
    [SerializeField] BattleLogSystem battleLogSystem;
    [SerializeField] RingPanel ringPanel;
    [SerializeField] GameObject actionPanel;
    [SerializeField] Transform focusImage;
    private BattleAction battleAction;
    public Character Actor;
    public Character Target;

    public void OnPanelClick(Character target)
    {
        if (IsTargetSelectMode)
        {
            Target = target;
            actionPanel.SetActive(false);
            StartResolve();
            IsTargetSelectMode = false ;
        }
        else
        {
            ringPanel.LoadRings(target);
            ringPanel.gameObject.SetActive(true);
        }
    }

    public void StartResolve()
    {
        focusImage.gameObject.SetActive(false);
        battleAction = BattleActionSO.GetAction(Actor, Target);
        battleLogSystem.ShowBattleAction(battleAction, ProcessBattleAction, true);
    }

    private void ProcessBattleAction()
    {
        battleAction.Resolve();
        // Battle Action Processed
        battleLogSystem.ShowActionResult(battleAction, ActionResolved, true);
    }

    private void ActionResolved()
    {
        BattleSystem.I.State = BattleState.Idle;
    }
}
