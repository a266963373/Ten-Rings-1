using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.SceneManagement;

public class CompetePanel : MonoBehaviour
{
    [SerializeField] GameObject confirmPanel;
    [SerializeField] LocalizeStringEvent confirmLse;
    private LevelSO level;
    private bool isUnlocked = false;
    private int unlockCost = 0;
    private LevelButton levelButton;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        confirmPanel.SetActive(false);
    }

    public void ShowCompetePanel(bool isUnlocked = false)
    {
        string localizedLevelName = new LocalizedString("Level Name", level.Name).GetLocalizedString();
        unlockCost = GameSystem.I.CurrentSave.UnlockedLevelNumber * 20;
        confirmLse.StringReference.TableEntryReference = isUnlocked ? "Level Enter" : "Level Unlock";
        confirmLse.StringReference.Add("LevelName", new StringVariable { Value = localizedLevelName });
        confirmLse.StringReference.Add("Gold", new IntVariable { Value = unlockCost });
        confirmPanel.SetActive(true);
    }

    public void OnClickLevelButton(LevelButton calledLevelButton)
    {
        levelButton = calledLevelButton;
        level = levelButton.level;
        isUnlocked = levelButton.IsUnlocked;
        ShowCompetePanel(isUnlocked);
    }

    public void OnClickYes()
    {
        if (isUnlocked)
        {
            GameSystem.I.Run.Level = level;
            SceneManager.LoadScene("LevelScene");
        } else
        {
            if (GameSystem.I.CurrentSave.Gold >= unlockCost)
            {
                GameSystem.I.CurrentSave.UnlockedLevelIds.Add(level.Id);
                levelButton.Unlock();
                OnClickNo();
            } else
            {
                confirmLse.StringReference.TableEntryReference = "Level Unlock Failed";
            }
        }
    }

    public void OnClickNo()
    {
        confirmPanel.SetActive(false);
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }
}
