using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using System.Linq;

public class SaveButton : MonoBehaviour
{
    [SerializeField] int id = 0;
    [SerializeField] LocalizeStringEvent lse;
    private int ringNumber;
    private int gold;
    private SaveData saveData;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // Ensure SaveSystem is initialized
        saveData = SaveSystem.I.Load(id);
        ringNumber = System.Linq.Enumerable.Count(saveData.WornRingIds, id => id != 0) + saveData.StoredRingIds.Count;
        gold = saveData.Gold;
        lse.StringReference.Clear();
        lse.StringReference.Add("RingNumber", new IntVariable { Value = ringNumber });
        lse.StringReference.Add("Gold", new IntVariable { Value = gold });
        lse.StringReference.SetReference("UI Text", "Save Detail");
        lse.RefreshString();
    }

    public void SaveButtonClicked()
    {
        GameSystem.I.LoadSaveData(id);
    }
}
