using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class StatusButton : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent lse;
    private LocalizedString locString;
    private Status status;

    public void Setup(Status _status)
    {
        status = _status;
        string localizedString = new LocalizedString("Status Name", status.Name).GetLocalizedString();
        if (status.IsStackable)
        {
            locString = new("UI Text", "Status Stackable")
            {
                Arguments = new object[] { localizedString, status.Stack }
            };
        } else
        {
            locString = new("UI Text", "Status Non Stackable")
            {
                Arguments = new object[] { localizedString }
            };
        }
        lse.StringReference = locString;
    }

    public void OnClick()
    {
        BattleSystem.I.StatusDescriptionPanel.ShowStatus(status);
    }
}
