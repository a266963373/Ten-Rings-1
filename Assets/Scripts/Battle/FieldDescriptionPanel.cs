using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class FieldDescriptionPanel : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent nameLse;
    [SerializeField] LocalizeStringEvent descriptionLse;

    private void Awake()
    {
        Hide();
    }

    private void SetField(Field field)
    {
        nameLse.StringReference.SetReference("Field", field.Name);
        descriptionLse.StringReference.SetReference("Field Description", field.Name);
        descriptionLse.StringReference.Add("Power", new IntVariable { Value = field.Power });
    }

    public void ShowField(Field field)
    {
        SetField(field);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
