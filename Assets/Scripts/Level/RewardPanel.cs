using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] RingButton ringButton1;
    [SerializeField] RingButton ringButton2;
    [SerializeField] GoldButton goldButton;
    [SerializeField] RingDescriptionPanel ringDescriptionPanel;
    [SerializeField] RingDescriptionText ringDescriptionText;
    [SerializeField] GameObject actionDescriptionGO;
    [SerializeField] LocalizeStringEvent claimLSE;

    private void Start()
    {
        List<RingSO> rings = RingLibrary.I.GetRandomRings(2);
        ringButton1.Ring = rings[0];
        ringButton1.OnClickAction += RingSelected;

        ringButton2.Ring = rings[1];
        ringButton2.OnClickAction += RingSelected;

        goldButton.Gold = BattleSession.Encounter.Gold;
        goldButton.OnClickAction += GoldSelected;
    }

    private void RingSelected(RingButton ringButton)
    {
        ringDescriptionPanel.Ring = ringButton.Ring;
        claimLSE.StringReference.SetReference("UI Text", "Borrow!");
    }

    private void GoldSelected()
    {
        ringDescriptionText.GoldSelected();
        actionDescriptionGO.SetActive(false);
        claimLSE.StringReference.SetReference("UI Text", "Claim!");
    }
}
