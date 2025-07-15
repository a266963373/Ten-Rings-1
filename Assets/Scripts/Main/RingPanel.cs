using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public enum InScene { Menu, Level, Battle }

public class RingPanel : MonoBehaviour
{
    [SerializeField] Transform wornTransform;
    [SerializeField] Transform storedTransform;
    [SerializeField] RingButton ringButton;
    [SerializeField] RingDescriptionPanel ringDescriptionPanel;
    [SerializeField] InScene InScene = InScene.Menu;
    [SerializeField] Transform focusImage;
    [SerializeField] CharacterInfoPanel characterInfoPanel;

    private List<RingSO> wornRings = new();
    private List<RingSO> storedRings = new();   // or display stats in battle scene

    private RingButton focusButton = null;

    private void Awake()
    {
        focusImage.gameObject.SetActive(false);
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameSystem.I.IsStarted);
        LoadRings();
    }

    public void LoadRings(Character character = null)
    {
        if (InScene == InScene.Level)
        {
            UseIdToLoadListAndTransform(GameSystem.I.Run.WornRingIds, wornRings, wornTransform);
            UseIdToLoadListAndTransform(GameSystem.I.Run.StoredRingIds, storedRings, storedTransform);
        }
        else if (InScene == InScene.Menu)
        {
            UseIdToLoadListAndTransform(GameSystem.I.CurrentSave.WornRingIds, wornRings, wornTransform);
            UseIdToLoadListAndTransform(GameSystem.I.CurrentSave.StoredRingIds, storedRings, storedTransform);
        }
        else
        {
            if (character == null) return;
            LoadCharacterInfo(character);
        }
    }

    private void SaveRings()
    {
        if (InScene == InScene.Level)
        {
            ExtractIdsFromRingList(wornRings, GameSystem.I.Run.WornRingIds);
            ExtractIdsFromRingList(storedRings, GameSystem.I.Run.StoredRingIds);
        }
        else
        {
            ExtractIdsFromRingList(wornRings, GameSystem.I.CurrentSave.WornRingIds);
            ExtractIdsFromRingList(storedRings, GameSystem.I.CurrentSave.StoredRingIds);
        }
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

    private void LoadCharacterInfo(Character character)
    {
        characterInfoPanel.SetCharacter(character);

        // don't destroy focusButton
        focusImage.transform.SetParent(transform, false);
        focusImage.gameObject.SetActive(false);
        ringDescriptionPanel.gameObject.SetActive(false);

        foreach (Transform child in wornTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (RingSO ring in character.Rings)
        {
            RingButton newRingButton = Instantiate(ringButton, wornTransform);
            newRingButton.Ring = ring;
            newRingButton.OnClickAction = RingButtonClicked;
        }
    }

    private void RingButtonClicked(RingButton ringButton)
    {
        ringDescriptionPanel.gameObject.SetActive(true);
        focusImage.gameObject.SetActive(true);
        focusImage.SetParent(ringButton.transform, false);
        focusImage.SetSiblingIndex(0);
        if (focusButton != ringButton)
        {
            focusButton = ringButton;
            ringDescriptionPanel.Ring = ringButton.Ring;
        }
        else
        {
            if (InScene != InScene.Battle)
            {
                TransferRing(ringButton);
                SaveRings();
            }
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

    public void ExitOnClick()
    {
        gameObject.SetActive(false);
        focusButton = null;
        focusImage.gameObject.SetActive(false);
    }

}
