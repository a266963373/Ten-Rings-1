using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class GoldDisplayer : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    private IntVariable goldVariable = new IntVariable { Value = -1 };

    IEnumerator Start()
    {
        while (GameSystem.I.CurrentSave == null)
        {
            yield return null;
        }
        GameSystem.I.OnGoldChange += SetGold;
        GameSystem.I.ModifyGold(0);
        localizeStringEvent.StringReference.Add("Gold", goldVariable);
        localizeStringEvent.StringReference.SetReference("UI Text", "Some Gold");
    }

    public void SetGold(int number)
    {
        goldVariable.Value = number;
    }

}
