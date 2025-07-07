using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowInfo(Character c)
    {
        string finalText = "Rings: " + c.Rings.Count.ToString();
        finalText += "\nSTR: " + c.Stats.GetStat(StatType.STR);
        text.text = finalText;
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
