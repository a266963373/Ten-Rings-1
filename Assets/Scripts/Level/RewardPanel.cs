using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] RingButton ringButton1;
    [SerializeField] RingButton ringButton2;
    [SerializeField] GoldButton goldButton;
    [SerializeField] RingDescriptionPanel ringDescriptionPanel;
    [SerializeField] RingDescriptionText ringDescriptionText;
    [SerializeField] GameObject actionDescriptionGO;
    [SerializeField] LocalizeStringEvent claimLSE;
    
    [SerializeField] GameObject congratsPanel;
    [SerializeField] Image backgroundImage;

    public Action OnClaimReward;
    public bool IsRingSelected = true;

    private void Awake()
    {
        claimLSE.gameObject.SetActive(false);
        ringDescriptionPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        List<Ring> rings = RingLibrary.I.GetRandomRings(2);
        ringButton1.Ring = rings[0];
        ringButton1.OnClickAction += RingSelected;

        ringButton2.Ring = rings[1];
        ringButton2.OnClickAction += RingSelected;

        goldButton.Gold = BattleSession.Encounter.Gold;
        goldButton.OnClickAction += GoldSelected;
    }

    private void RingSelected(RingButton ringButton)
    {
        ringDescriptionPanel.gameObject.SetActive(true);
        IsRingSelected = true;
        ringDescriptionPanel.Ring = ringButton.Ring;
        if (BattleSession.IsLastEncounter)
        {
            claimLSE.StringReference.SetReference("UI Text", "Claim!");
        }
        else
        {
            claimLSE.StringReference.SetReference("UI Text", "Borrow!");
        }
        claimLSE.gameObject.SetActive(true);
    }

    private void GoldSelected()
    {
        ringDescriptionPanel.gameObject.SetActive(true);
        IsRingSelected = false;
        ringDescriptionText.GoldSelected();
        actionDescriptionGO.SetActive(false);
        claimLSE.StringReference.SetReference("UI Text", "Claim!");
        claimLSE.gameObject.SetActive(true);
    }

    public void ClaimReward()
    {
        if (IsRingSelected)
        {
            if (ringDescriptionPanel.Ring == null)
            {
                return;
            }

            if (!BattleSession.IsLastEncounter)
            {
                GameSystem.I.Run.StoredRingIds.Add(ringDescriptionPanel.Ring.Id);
            }
            else
            {
                GameSystem.I.CurrentSave.StoredRingIds.Add(ringDescriptionPanel.Ring.Id);
            }
        }
        else
        {
            GameSystem.I.ModifyGold(BattleSession.EncounterGold);
        }
        if (BattleSession.IsLastEncounter)
        {
            if (BattleSession.Encounter.Name == "03.05")
            {
                // final level, show Congrats
                backgroundImage.sprite = Resources.Load<Sprite>("Images/Level & Battle Backgrounds/Congrats");
                congratsPanel.SetActive(true);
            }
            else
            {
                LoadMainScene();
                return;
            }
        }
        OnClaimReward();
        IsRingSelected = false;
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
