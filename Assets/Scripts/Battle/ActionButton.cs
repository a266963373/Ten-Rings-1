using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ActionButton : MonoBehaviour
{
    private BattleActionSO battleAction;
    [SerializeField] TextMeshProUGUI text;
    private ActionResolver resolver;

    public void Initialize(BattleActionSO b, ActionResolver r)
    {
        battleAction = b;
        resolver = r;
        text.text = battleAction.name;
    }

    public void OnClick()
    {
        resolver.IsTargetSelectMode = true;
        resolver.BattleActionSO = battleAction;
    }
}
