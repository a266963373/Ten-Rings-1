using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using System;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class BattleLogSystem : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI messageText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    [SerializeField] ActionDecider actionDecider;   // OnClick related
    //private Action onContinue;
    private bool isClicked = false;
    private bool IsClicked
    {
        get { 
            if (isClicked)
            {
                isClicked = false;
                return true;
            }
            return false;
        }
    }

    public IEnumerator ShowMessage(LocalizedString localizedString, bool isBlock=true)
    {
        localizeStringEvent.StringReference = localizedString;

        if (isBlock)
        {
            yield return new WaitUntil(() => IsClicked);
        }
    }

    public IEnumerator ShowWhoseTurn(string name, bool isBlock = true)
    {
        LocalizedString locString = new("Battle Log", "ShowWhoseTurn");
        locString.Arguments = new object[] { new LocalizedString("Character Name", name).GetLocalizedString() };
        yield return ShowMessage(locString, isBlock);
    }

    public IEnumerator ShowBattleAction(BattleAction battleAction, bool isBlock = true)
    {
        LocalizedString locString = new("Battle Log", "ShowBattleAction");
        string localizedActorName = new LocalizedString("Character Name", battleAction.Actor.Name).GetLocalizedString();
        string localizedTargetName = new LocalizedString("Character Name", battleAction.Target.Name).GetLocalizedString();
        string localizedBattleActionName = new LocalizedString("Battle Action", battleAction.Name).GetLocalizedString();

        locString.Arguments = new object[] { localizedActorName, localizedTargetName, localizedBattleActionName };
        yield return ShowMessage(locString, isBlock);

        if (battleAction.HasExtraLog)
        {
            locString = new("Extra Log", battleAction.Name);
            string relatedActionName = new LocalizedString("Battle Action", battleAction.RelatedAction.Name).GetLocalizedString();
            locString.Add("RelatedAction", new StringVariable { Value = relatedActionName });
            yield return ShowMessage(locString, isBlock);
        }
    }

    public IEnumerator ShowActionResult(BattleAction battleAction, bool isBlock = true)
    {
        LocalizedString locString = new("Battle Log", "ShowActionResult");
        string localizedTargetName = new LocalizedString("Character Name", battleAction.Target.Name).GetLocalizedString();
        locString.Arguments = new object[] { localizedTargetName, battleAction.Damage.Value };
        yield return ShowMessage(locString, isBlock);
    }

    public void OnClick()
    {
        //var callback = onContinue;
        //onContinue = null;
        //callback?.Invoke();
        
        isClicked = true;
        actionDecider.RemoveFocusAction();
    }
}
