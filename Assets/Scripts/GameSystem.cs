using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System.Linq;

public class GameSystem : MonoBehaviour
{
    public static GameSystem I { get; private set; }

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
    }

    public SaveData CurrentSave { get; private set; }

    public void SaveSaveData(int saveId)
    {
        SaveSystem.I.Save(CurrentSave);
    }

    public void LoadSaveData(int saveId)
    {
        CurrentSave = SaveSystem.I.Load(saveId);
        SceneManager.LoadScene("MainScene");
    }
}
