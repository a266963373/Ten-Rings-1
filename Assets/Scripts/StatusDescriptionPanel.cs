using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class StatusDescriptionPanel : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent nameLse;
    [SerializeField] LocalizeStringEvent descriptionLse;

    private void SetStatus(Status status)
    {
        nameLse.StringReference.TableEntryReference = status.Name;
        descriptionLse.StringReference.TableEntryReference = status.Name;
        descriptionLse.StringReference.Add("Power", new IntVariable { Value = status.Power });
    }

    public void ShowStatus(Status status)
    {
        SetStatus(status);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
