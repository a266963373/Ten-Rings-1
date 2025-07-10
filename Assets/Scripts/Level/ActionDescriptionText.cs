using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class ActionDescriptionText : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    // for LSE arguments
    public int Value;
    public string Form;
    public string Element;

    private Damage damage;
    private BattleActionSO battleAction;
    public BattleActionSO BattleAction
    {
        get { return battleAction; }
        set
        {
            battleAction = value;
            damage = battleAction.Damage;
            Value = damage.Value;
            Form = new LocalizedString("Damage Form", damage.Form.ToString()).GetLocalizedString();
            Element = new LocalizedString("Damage Element", damage.Element.ToString()).GetLocalizedString();
            localizeStringEvent.StringReference.Add("Value", new IntVariable { Value = Value });
            localizeStringEvent.StringReference.Add("Form", new StringVariable { Value = Form });
            localizeStringEvent.StringReference.Add("Element", new StringVariable { Value = Element });

            Show();
        }
    }

    private void Show()
    {
        localizeStringEvent.StringReference.SetReference("Action Description", battleAction.Name);
    }
}
