using System;
using System.Collections;
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
            StartCoroutine(StartResolve());
            IsTargetSelectMode = false ;
        }
        else
        {
            ringPanel.LoadRings(target);
            ringPanel.gameObject.SetActive(true);
        }
    }

    public IEnumerator StartResolve()
    {
        focusImage.gameObject.SetActive(false);
        battleAction = BattleActionSO.GetAction(Actor, Target);
        yield return battleLogSystem.ShowBattleAction(battleAction, true);
        yield return ProcessBattleAction();
    }

    private IEnumerator ProcessBattleAction()
    {
        // Battle Action Processed
        yield return battleLogSystem.ShowActionResult(battleAction.Resolve(), true);
        ActionResolved();
    }

    private void ActionResolved()
    {
        BattleSystem.I.State = BattleState.Idle;
    }
}
