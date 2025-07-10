using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class GoldButton : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    public Action OnClickAction;
    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            localizeStringEvent.StringReference.Add("Gold", new IntVariable { Value = gold });
            localizeStringEvent.StringReference.SetReference("UI Text", "Some Gold");
        }
    }

    public void OnClick()
    {
        OnClickAction?.Invoke();
    }
}
