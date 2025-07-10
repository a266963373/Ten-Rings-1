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
    [SerializeField] ActionDecider actionDecider;   // OnClick related
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
        locString.Arguments = new object[] { new LocalizedString("Character Name", name).GetLocalizedString() };
        ShowMessage(locString, onContinueCallback, isBlock);
    }

    public void ShowBattleAction(BattleAction battleAction, Action onContinueCallback = null, bool isBlock = true)
    {
        LocalizedString locString = new LocalizedString("Battle Log", "ShowBattleAction");
        string localizedActorName = new LocalizedString("Character Name", battleAction.Actor.Name).GetLocalizedString();
        string localizedTargetName = new LocalizedString("Character Name", battleAction.Target.Name).GetLocalizedString();
        string localizedBattleActionName = new LocalizedString("Battle Action", battleAction.Name).GetLocalizedString();

        locString.Arguments = new object[] { localizedActorName, localizedTargetName, localizedBattleActionName };
        ShowMessage(locString, onContinueCallback, isBlock);
    }

    public void ShowActionResult(BattleAction battleAction, Action onContinueCallback = null, bool isBlock = true)
    {
        LocalizedString locString = new LocalizedString("Battle Log", "ShowActionResult");
        string localizedTargetName = new LocalizedString("Character Name", battleAction.Target.Name).GetLocalizedString();
        locString.Arguments = new object[] { localizedTargetName, battleAction.Damage.Value };
        ShowMessage(locString, onContinueCallback, isBlock);
    }

    public void OnClick()
    {
        var callback = onContinue;
        onContinue = null;
        callback?.Invoke();

        actionDecider.RemoveFocusAction();
    }
}
