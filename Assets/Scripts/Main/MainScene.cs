using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject savePanel;
    [SerializeField] GameObject ringPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject competePanel;

    private void Start()
    {
        menuPanel.SetActive(true);
        savePanel.SetActive(false);
        ringPanel.SetActive(false);
        shopPanel.SetActive(false);
        competePanel.SetActive(false);
    }

    public void ToggleMenuPanel()
    {
        bool isActive = menuPanel.activeSelf;
        menuPanel.SetActive(!isActive);
    }

    public void ToggleSavePanel()
    {
        bool isActive = savePanel.activeSelf;
        savePanel.SetActive(!isActive);

        ToggleMenuPanel();
    }

    public void ToggleRingPanel()
    {
        bool isActive = ringPanel.activeSelf;
        ringPanel.SetActive(!isActive);

        ToggleMenuPanel();
    }

    public void ToggleShopPanel()
    {
        bool isActive = shopPanel.activeSelf;
        shopPanel.SetActive(!isActive);

        ToggleMenuPanel();
    }

    public void ToggleCompetePanel()
    {
        bool isActive = competePanel.activeSelf;
        competePanel.SetActive(!isActive);

        ToggleMenuPanel();
    }

    public void SaveButtonClicked(int id)
    {
        GameSystem.I.SaveSaveData(id);
        ToggleSavePanel();
    }

    public void LoadButtonClicked(int id)
    {
        GameSystem.I.LoadSaveData(id);
        ToggleSavePanel();
    }
}
