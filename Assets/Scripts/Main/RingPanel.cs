using System.Collections.Generic;
using UnityEngine;

public class RingPanel : MonoBehaviour
{
    [SerializeField] Transform wornTransform;
    [SerializeField] Transform storedTransform;
    [SerializeField] RingButton ringButton;
    [SerializeField] RingDescriptionPanel ringDescriptionPanel;

    private List<RingSO> wornRings = new();
    private List<RingSO> storedRings = new();

    private RingButton focusButton = null;

    private void Start()
    {
        LoadCurrentSave();
    }

    private void LoadCurrentSave()
    {
        UseIdToLoadListAndTransform(GameSystem.I.CurrentSave.WornRingIds, wornRings, wornTransform);
        UseIdToLoadListAndTransform(GameSystem.I.CurrentSave.StoredRingIds, storedRings, storedTransform);
    }

    private void SaveCurrentSave()
    {
        ExtractIdsFromRingList(wornRings, GameSystem.I.CurrentSave.WornRingIds);
        ExtractIdsFromRingList(storedRings, GameSystem.I.CurrentSave.StoredRingIds);
    }

    private void UseIdToLoadListAndTransform(List<int> ids, List<RingSO> rings, Transform transform)
    {
        foreach (int id in ids)
        {
            RingSO newRing = RingLibrary.I.GetRingById(id);
            rings.Add(newRing);
            RingButton newRingButton = Instantiate(ringButton, transform);
            newRingButton.Ring = newRing;
            newRingButton.OnClickAction = RingButtonClicked;
        }
    }

    private void ExtractIdsFromRingList(List<RingSO> rings, List<int> ids)
    {
        ids.Clear(); // 可选：清空旧内容
        foreach (RingSO ring in rings)
        {
            ids.Add(ring.Id); // 假设 RingSO 有 public int Id 属性
        }
    }

    private void RingButtonClicked(RingButton ringButton)
    {
        if (focusButton != ringButton)
        {
            focusButton = ringButton;
            ringDescriptionPanel.Ring = ringButton.Ring;
        }
        else
        {
            TransferRing(ringButton);
            SaveCurrentSave();
        }
    }

    private void TransferRing(RingButton ringButton)
    {
        if (ringButton.transform.parent == wornTransform)
        {
            ringButton.transform.SetParent(storedTransform);
            wornRings.Remove(ringButton.Ring);
            storedRings.Add(ringButton.Ring);
        }
        else
        {
            ringButton.transform.SetParent(wornTransform);
            storedRings.Remove(ringButton.Ring);
            wornRings.Add(ringButton.Ring);
        }
    }

}
