using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class PenaltyPanel : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent lse;

    private void Awake()
    {
        lse.gameObject.SetActive(false);
    }

    private EncounterSO encounter;
    public EncounterSO Encounter
    {
        get { return encounter; }
        set
        {
            encounter = value;
            lse.StringReference.Add("Gold", new IntVariable { Value = encounter.Gold });
            lse.StringReference.SetReference("System Message", "Penalty Message");
            lse.gameObject.SetActive(true);
            Penalize();
        }
    }

    private void Penalize()
    {
        GameSystem.I.ModifyGold(-encounter.Gold);
    }
}
