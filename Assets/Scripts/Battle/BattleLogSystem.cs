using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using System;

public class BattleLogSystem : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI messageText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    private Action onContinue;

    public void ShowMessage(LocalizedString localizedString, Action onContinueCallback=null, bool isBlock=true)
    {
        localizeStringEvent.StringReference = localizedString;

        if (isBlock)
        {
            onContinue = onContinueCallback;
        } else
        {
            onContinueCallback?.Invoke();
        }
    }

    public void ShowWhoseTurn(string name, Action onContinueCallback = null, bool isBlock = true)
    {
        LocalizedString locString = new LocalizedString("Battle Log", "ShowWhoseTurn");
        locString.Arguments = new object[] { name };
        ShowMessage(locString, onContinueCallback, isBlock);
    }

    public void ShowBattleAction(BattleAction battleAction, Action onContinueCallback = null, bool isBlock = true)
    {
        LocalizedString locString = new LocalizedString("Battle Log", "ShowBattleAction");
        locString.Arguments = new object[] { battleAction.Actor.Name, battleAction.Target.Name, battleAction.Name };
        ShowMessage(locString, onContinueCallback, isBlock);
    }

    public void ShowActionResult(BattleAction battleAction, Action onContinueCallback = null, bool isBlock = true)
    {
        LocalizedString locString = new LocalizedString("Battle Log", "ShowActionResult");
        locString.Arguments = new object[] { battleAction.Target.Name, battleAction.Damage.Value };
        ShowMessage(locString, onContinueCallback, isBlock);
    }

    public void OnClick()
    {
        var callback = onContinue;
        onContinue = null;
        callback?.Invoke();
    }
}
