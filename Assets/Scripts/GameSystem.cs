using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System;
using System.Linq;

public class GameSystem : MonoBehaviour
{
    public static GameSystem I { get; private set; }
    public SaveData CurrentSave { get; private set; }
    public Action<int> OnGoldChange;

    void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // for devs
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales
            .FirstOrDefault(l => l.Identifier.Code == "en");

        if (CurrentSave == null)
        {
            UseDebugSave();
        }
    }

    public void SaveSaveData(int saveId)
    {
        SaveSystem.I.Save(CurrentSave, saveId);
    }

    public void LoadSaveData(int saveId)
    {
        CurrentSave = SaveSystem.I.Load(saveId);
        SceneManager.LoadScene("MainScene");
    }

    private void UseDebugSave()
    {
        CurrentSave = SaveSystem.I.Load(1);
    }

    public void ModifyGold(int modifyingNum)
    {
        CurrentSave.Gold += modifyingNum;
        OnGoldChange?.Invoke(CurrentSave.Gold);
    }

}
