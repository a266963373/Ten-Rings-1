using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public LevelSO level;
    [SerializeField] Image image;
    [SerializeField] GameObject lockedFrontground;
    public bool IsUnlocked => !lockedFrontground.activeSelf;

    private void Start()
    {
        image.sprite = level.BackgroundImage;
        if (GameSystem.I.CurrentSave.UnlockedLevelIds.Contains(level.Id))
        {
            Unlock();
        }
        else
        {
            Lock();
        }
    }

    public void Lock()
    {
        lockedFrontground.SetActive(true);
    }

    public void Unlock()
    {
        lockedFrontground.SetActive(false);
    }
}
