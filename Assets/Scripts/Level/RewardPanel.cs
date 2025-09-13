using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] RingButton ringButton1;
    [SerializeField] RingButton ringButton2;
    [SerializeField] GoldButton goldButton;
    [SerializeField] RingDescriptionPanel ringDescriptionPanel;
    [SerializeField] RingDescriptionText ringDescriptionText;
    [SerializeField] GameObject actionDescriptionGO;
    [SerializeField] LocalizeStringEvent claimLSE;
    public Action OnClaimReward;
    public bool IsRingSelected = true;

    private void Awake()
    {
        claimLSE.gameObject.SetActive(false);
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
            SceneManager.LoadScene("MainScene");
            return;
        }
        OnClaimReward();
        IsRingSelected = false;
    }
}
