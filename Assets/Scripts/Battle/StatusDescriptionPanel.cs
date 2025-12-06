using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class StatusDescriptionPanel : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent nameLse;
    [SerializeField] LocalizeStringEvent descriptionLse;
    [SerializeField] LocalizeStringEvent statLse;

    private void SetStatus(Status status)
    {
        nameLse.StringReference.TableEntryReference = status.Name;
        descriptionLse.StringReference.TableEntryReference = status.Name;
        descriptionLse.StringReference.Add("Power", new IntVariable { Value = status.Power });
        descriptionLse.StringReference.Add("Power2", new IntVariable { Value = status.Power2 });
        descriptionLse.StringReference.Add("Value", new IntVariable { Value = status.CommonEffectValue() });

        string localizedStatName = new LocalizedString("Stat Type", status.DecayStatType.ToString()).GetLocalizedString();
        statLse.StringReference.Add("Stat", new StringVariable { Value = localizedStatName });

        if (status.UpkeepMana != 0)
        {
            statLse.StringReference.Add("Upkeep", new IntVariable { Value = status.UpkeepMana});
            statLse.StringReference.TableEntryReference = "Status Stat Upkeep";
        }
        else
        {
            statLse.StringReference.TableEntryReference = "Status Stat";
        }
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
