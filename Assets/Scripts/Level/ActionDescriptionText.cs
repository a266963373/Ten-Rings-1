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
    //public int Value;
    public string Form;
    public string Element;

    private Damage damage;
    private BattleActionSO battleAction;
    public BattleActionSO BattleAction
    {
        //get { return battleAction; }
        set
        {
            if (value is ActivateWeaponActionSO activateWeaponActionSO)
            {
                BattleAction = activateWeaponActionSO.GrantedActions[0];
            } else
            {
                battleAction = value;
                damage = value.Damage;
                Form = new LocalizedString("Damage Form", damage.Form.ToString()).GetLocalizedString();
                Element = new LocalizedString("Damage Element", damage.Element.ToString()).GetLocalizedString();
                localizeStringEvent.StringReference.Add("Value", new IntVariable { Value = Mathf.Abs(damage.Value) });
                localizeStringEvent.StringReference.Add("Form", new StringVariable { Value = Form });
                localizeStringEvent.StringReference.Add("Element", new StringVariable { Value = Element });

                if (battleAction.StatusStacks.Count > 0)
                {
                    localizeStringEvent.StringReference.Add("Stack", new IntVariable { Value = battleAction.StatusStacks[0] });
                }

                if (battleAction is FieldActionSO fa)
                {
                    localizeStringEvent.StringReference.Add("Duration", new IntVariable { Value = fa.FieldDuration });
                }

                for (int i = 0; i < value.StatusStacks.Count; i++)
                {
                    int stack = Mathf.Abs(value.StatusStacks[i]);
                    localizeStringEvent.StringReference.Add($"Stack {stack}", new IntVariable { Value = stack });
                }

                Show();
            }
        }
    }

    private void Show()
    {
        localizeStringEvent.StringReference.SetReference("Action Description", battleAction.Name);
        localizeStringEvent.RefreshString();
    }
}
