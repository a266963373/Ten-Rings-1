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
    public static BattleLogSystem I;
    private void Awake()
    {
        I = this;
    }
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
    //public bool IsShowingMessage = false;

    public IEnumerator ShowMessage(LocalizedString localizedString, bool isBlock=true)
    {
        localizeStringEvent.StringReference = localizedString;
        localizeStringEvent.RefreshString();

        if (isBlock)
        {
            //IsShowingMessage = true;
            isClicked = false;  // necessary because player might clicked before using skill to make it positive
            yield return new WaitUntil(() => IsClicked);
            //IsShowingMessage = false;
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
        LocalizedString locString;
        string localizedActorName = new LocalizedString("Character Name", battleAction.Actor.Name).GetLocalizedString();
        string localizedBattleActionName = new LocalizedString("Battle Action", battleAction.Name).GetLocalizedString();

        if (battleAction.Name == "Activate")
        {
            locString = new("Battle Log", "ShowActivateWeapon");
            locString.Arguments = new object[] { localizedActorName };
        }
        else if (battleAction.IsRemovingStatuses)
        {
            locString = new("Battle Log", "ShowCeaseAction");
            locString.Arguments = new object[] { localizedActorName, localizedBattleActionName };
        }
        else if (battleAction.IsMustSelf || battleAction.Target == null)
        {
            locString = new("Battle Log", "ShowSelfAction");
            locString.Arguments = new object[] { localizedActorName, localizedBattleActionName };
        }
        else
        {
            locString = new("Battle Log", "ShowBattleAction");
            string localizedTargetName = new LocalizedString("Character Name", battleAction.Target.Name).GetLocalizedString();
            locString.Arguments = new object[] { localizedActorName, localizedTargetName, localizedBattleActionName };
        }

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
        LocalizedString locString;
        string localizedTargetName = new LocalizedString("Character Name", battleAction.Target.Name).GetLocalizedString();
        if (battleAction.Range == RangeType.Indirect)
        {
            locString = new("Battle Log", "ShowIndirectActionResult");
            string localizedSourceName = new LocalizedString("Battle Action", battleAction.Name).GetLocalizedString();
            locString.Arguments = new object[] { localizedTargetName, battleAction.Damage.Value, localizedSourceName };
        }
        else if (!battleAction.Damage.IsHealing)
        {
            locString = new("Battle Log", "ShowResultDamage")
            {
                Arguments = new object[] { localizedTargetName, battleAction.Damage.Value }
            };
        }
        else
        {
            locString = new("Battle Log", "ShowResultHeal")
            {
                Arguments = new object[] { localizedTargetName, -battleAction.Damage.Value }
            };
        }
        yield return ShowMessage(locString, isBlock);
    }

    public IEnumerator ShowActionCheck(BattleAction battleAction, bool isBlock = true)
    {
        LocalizedString locString;
        if (battleAction.IsMissed)
        {
            locString = new LocalizedString("Battle Log", "ShowActionMissed");
        }
        else if (battleAction.IsCrit)
        {
            locString = new LocalizedString("Battle Log", "ShowActionCrit");
        }
        else if (battleAction.IsBlocked)
        {
            locString = new LocalizedString("Battle Log", "ShowActionBlocked");
        }
        else
        {
            yield break;
        }

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
