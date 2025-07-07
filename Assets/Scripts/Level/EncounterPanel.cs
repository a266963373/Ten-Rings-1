using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

public class EncounterPanel : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizedCharacterName;
    [SerializeField] LocalizeStringEvent localizedEncounterDescription;

    private EncounterSO encounter;
    public EncounterSO Encounter
    {
        get { return encounter; }
        set
        {
            encounter = value;

            localizedCharacterName.StringReference.SetReference("Encounter Name", encounter.Name);
            localizedEncounterDescription.StringReference.SetReference("Encounter Description", encounter.Name);
        }
    }

    public void EnterBattle()
    {
        BattleSession.Encounter = encounter;
        SceneManager.LoadScene("BattleScene");
    }
}
