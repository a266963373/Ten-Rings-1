using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class CharacterInfoPanel : MonoBehaviour     // not showing rings, that is RingPanel's job
{
    [SerializeField] LocalizeStringEvent nameLSE;
    [SerializeField] LocalizeStringEvent strengthLSE;
    [SerializeField] LocalizeStringEvent mindLSE;
    [SerializeField] LocalizeStringEvent speedLSE;

    public void SetCharacter(Character c)
    {
        nameLSE.StringReference.SetReference("Character Name", c.Name);

        strengthLSE.StringReference.SetReference("Stat", "Strength");
        strengthLSE.StringReference.Arguments = new object[] { c.Stats.GetStat(StatType.STR) };
        mindLSE.StringReference.SetReference("Stat", "Mind");
        mindLSE.StringReference.Arguments = new object[] { c.Stats.GetStat(StatType.MND) };
        speedLSE.StringReference.SetReference("Stat", "Speed");
        speedLSE.StringReference.Arguments = new object[] { c.Stats.GetStat(StatType.SPD) };

    }

}
