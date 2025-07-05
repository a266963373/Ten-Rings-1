using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using System.Linq;

public class TitleScene : MonoBehaviour
{
    private GameSettings settings;
    private void Start()
    {
        settings = SettingsManager.Load();
        SetLanguage(settings.language);
    }

    public void GoToSavesScene()
    {
        SceneManager.LoadScene("SaveScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    [SerializeField] Transform languagePanel;
    public void ToggleLanguagePanel()
    {
        bool isActive = languagePanel.gameObject.activeSelf;
        languagePanel.gameObject.SetActive(!isActive);
    }

    public void SetLanguage(string localeCode)
    {
        StartCoroutine(SetLanguageCoroutine(localeCode));
    }

    private IEnumerator SetLanguageCoroutine(string code)
    {
        yield return LocalizationSettings.InitializationOperation;

        var locale = LocalizationSettings.AvailableLocales.Locales
            .FirstOrDefault(l => l.Identifier.Code == code);

        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            settings.language = code;
            SettingsManager.Save(settings);
        } else
        {
            Debug.LogError($"Set language failed: {code}");
        }
    }
}
