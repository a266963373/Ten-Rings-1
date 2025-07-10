using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent dialogueLSE;
    [SerializeField] LocalizeStringEvent buttonLSE;
    [SerializeField] LocalizeStringEvent priceLSE;
    [SerializeField] Transform goodsPanel;
    [SerializeField] RingButton ringButton;
    [SerializeField] Transform focusImage;
    private ShopSetting shopSetting;
    private RingButton focusButton = null;
    private int sponsorPrice = 50;
    private int ringPrice { get { return 50 + shopSetting.ShoppedNumber * 5; } }
    private bool isRingSelected = true;
    private IntVariable priceVariable = new IntVariable();
    private bool isYeah = false;

    private void Awake()
    {
        focusImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        shopSetting = GameSystem.I.CurrentSave.ShopSetting;
        ShowGoods();
        priceLSE.StringReference.Add("Price", priceVariable);
        UpdatePriceVariable(0);
        priceLSE.StringReference.SetReference("UI Text", "Price");
    }

    private void ShowGoods()
    {
        RingLibrary.I.FillRandomIds(shopSetting.SellingRingIds, shopSetting.ShopLevel * 3);

        foreach (int id in shopSetting.SellingRingIds)
        {
            RingSO newRing = RingLibrary.I.GetRingById(id);
            RingButton newRingButton = Instantiate(ringButton, goodsPanel);
            newRingButton.OnClickAction = RingButtonClicked;
            newRingButton.Ring = newRing;
        }
    }

    private void UpdatePriceVariable(int num = -1)
    {
        if (num == -1)
        {
            priceVariable.Value = isRingSelected ? ringPrice : sponsorPrice;
        }
        else
        {
            priceVariable.Value = num;
        }
        //priceLSE.RefreshString();
    }

    private void RingButtonClicked(RingButton ringButton)
    {
        isRingSelected = true;
        isYeah = false;
        focusButton = ringButton;
        focusImage.gameObject.SetActive(true);
        focusImage.SetParent(ringButton.transform, false);
        focusImage.SetSiblingIndex(0);
        dialogueLSE.StringReference.SetReference("Vendor Dialogue", "ring selected");
        buttonLSE.StringReference.SetReference("UI Text", "Deal!");
        UpdatePriceVariable();
    }

    public void SponsorButtonClicked()
    {
        isRingSelected = false;
        isYeah = false;
        focusButton = null;
        focusImage.gameObject.SetActive(false);
        if (shopSetting.SellingRingIds.Count < RingLibrary.I.RingCount)
        {
            dialogueLSE.StringReference.SetReference("Vendor Dialogue", "sponsored");
            buttonLSE.StringReference.SetReference("UI Text", "Deal!");
        }
        else
        {
            dialogueLSE.StringReference.SetReference("Vendor Dialogue", "ring all collected");
            buttonLSE.StringReference.SetReference("UI Text", "Yeah!");
            isYeah = true;
        }
        UpdatePriceVariable();
    }

    public void DealButtonClicked()
    {
        if (focusButton == null && isRingSelected)
        {
            dialogueLSE.StringReference.SetReference("Vendor Dialogue", "not selected");
            return;
        }

        if (GameSystem.I.CurrentSave.Gold > priceVariable.Value)
        {
            if (isRingSelected)
            {
                dialogueLSE.StringReference.SetReference("Vendor Dialogue", "dealed");
                GameSystem.I.ModifyGold(-priceVariable.Value);
                shopSetting.ShoppedNumber++;
                GameSystem.I.CurrentSave.StoredRingIds.Add(focusButton.Ring.Id);
            }
            else
            {
                if (isYeah) return;
                dialogueLSE.StringReference.SetReference("Vendor Dialogue", "sponsored");
                GameSystem.I.ModifyGold(-priceVariable.Value);
                shopSetting.ShopLevel++;
            }
            UpdatePriceVariable(0);
        }
        else
        {
            if (isYeah) return;
            dialogueLSE.StringReference.SetReference("Vendor Dialogue", "too poor");
        }
        focusButton = null;
        focusImage.gameObject.SetActive(false);
    }

}
