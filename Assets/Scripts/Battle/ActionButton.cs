using System;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    private BattleActionSO battleAction;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    [SerializeField] Button button;
    public Action<BattleActionSO, Transform> OnClickAction;

    public void Initialize(BattleActionSO b)
    {
        battleAction = b;
        localizeStringEvent.StringReference.SetReference("Battle Action", b.Name);
    }

    public void OnClick()
    {
        OnClickAction(battleAction, transform);
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}
