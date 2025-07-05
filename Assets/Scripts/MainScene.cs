using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    [SerializeField] Transform menuPanel;
    [SerializeField] Transform savePanel;

    private void Start()
    {
        menuPanel.gameObject.SetActive(true);
        savePanel.gameObject.SetActive(false);
    }

    public void ToggleMenuPanel()
    {
        bool isActive = menuPanel.gameObject.activeSelf;
        menuPanel.gameObject.SetActive(!isActive);
    }

    public void ToggleSavePanel()
    {
        bool isActive = savePanel.gameObject.activeSelf;
        savePanel.gameObject.SetActive(!isActive);

        ToggleMenuPanel();
    }
}
