using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    private BattleActionSO battleAction;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    [SerializeField] Button button;
    public Action<BattleActionSO, Transform> OnClickAction;

    public void Initialize(BattleActionSO b)
    {
        battleAction = b;
        if (b is ActivateWeaponActionSO a)
        {
            localizeStringEvent.StringReference.SetReference("Battle Action", "Activate");
            string localizedWeaponRingName = new LocalizedString("Ring Name", a.WeaponRingSO.Name).GetLocalizedString();
            localizeStringEvent.StringReference.Arguments = new object[] { localizedWeaponRingName };
        }
        else if (b is InvokeActionSO ia)
        {
            string localizedActionName = new LocalizedString("Battle Action", ia.Name).GetLocalizedString();
            if (!ia.IsActionInvoked)
            {
                localizeStringEvent.StringReference.SetReference("Battle Action", "Invoke");
            }
            else
            {
                localizeStringEvent.StringReference.SetReference("Battle Action", "Cease");
            }
            localizeStringEvent.StringReference.Arguments = new object[] { localizedActionName };
        }
        else
        {
            localizeStringEvent.StringReference.SetReference("Battle Action", b.Name);
        }
    }

    public void OnClick()
    {
        OnClickAction(battleAction, transform);
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}
