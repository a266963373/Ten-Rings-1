using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private Ring[] wornRings = new Ring[10];
    private List<Ring> storedRings = new();
    private RingButton focusButton = null;

    private void Awake() => focusImage.gameObject.SetActive(false);

    private IEnumerator Start()
    {
        while (!GameSystem.I.IsStarted)
            yield return new WaitUntil(() => GameSystem.I.IsStarted);
        if (InScene != InScene.Battle) LoadRings();     // otherwise it will wipe the rings on first start
    }

    public void LoadRings(Character character = null)
    {
        ClearRingsUI();
        System.Array.Clear(wornRings, 0, wornRings.Length);
        storedRings.Clear();

        if (InScene == InScene.Level)
        {
            LoadRingList(GameSystem.I.Run.WornRingIds, wornRings, wornTransform);
            LoadRingList(GameSystem.I.Run.StoredRingIds, storedRings, storedTransform);
        }
        else if (InScene == InScene.Menu)
        {
            LoadRingList(GameSystem.I.CurrentSave.WornRingIds, wornRings, wornTransform);
            LoadRingList(GameSystem.I.CurrentSave.StoredRingIds, storedRings, storedTransform);
        }
        else if (character != null)
        {
            LoadCharacterInfo(character);
        }
    }

    private void SaveRings()
    {
        if (InScene == InScene.Level)
        {
            SaveRingList(wornRings, GameSystem.I.Run.WornRingIds);
            SaveRingList(storedRings, GameSystem.I.Run.StoredRingIds);
        }
        else if (InScene == InScene.Menu)
        {
            SaveRingList(wornRings, GameSystem.I.CurrentSave.WornRingIds);
            SaveRingList(storedRings, GameSystem.I.CurrentSave.StoredRingIds);
        }
        else // Battle 或其他
        {
            // 可根据需要扩展
            Debug.LogWarning("Saving rings in Battle scene is not implemented.");
        }
    }

    private void LoadRingList(IEnumerable<int> ids, object rings, Transform parent)
    {
        int index = 0;

        if (rings is List<Ring> list)
        {
            foreach (int id in ids)
            {
                Ring ring = RingLibrary.I.GetRingById(id);
                CreateRingButton(ring, parent, index);
                list.Add(ring);
                index++;
            }
        }
        else if (rings is Ring[] array)
        {
            foreach (int id in ids)
            {
                Ring ring = RingLibrary.I.GetRingById(id);
                if (index < array.Length)
                    array[index] = ring;
                index++;
            }
            for (index = 0; index < array.Length; index++)
            {
                CreateRingButton(array[index], parent, index);
            }
        }
    }

    private void SaveRingList(object rings, object ids)
    {
        IEnumerable<Ring> ringEnumerable = null;
        if (rings is List<Ring> ringList)
            ringEnumerable = ringList;
        else if (rings is Ring[] ringArray)
            ringEnumerable = ringArray;
        else
        {
            Debug.LogError("Unsupported type for rings");
            return;
        }

        if (ids is List<int> idList)
        {
            idList.Clear();
            foreach (var ring in ringEnumerable)
                idList.Add(ring == null ? 0 : ring.Id);
        }
        else if (ids is int[] idArray)
        {
            System.Array.Clear(idArray, 0, idArray.Length);
            int i = 0;
            foreach (var ring in ringEnumerable)
                if (i < idArray.Length) idArray[i++] = ring == null ? 0 : ring.Id;
        }
        else
        {
            Debug.LogError("Unsupported type for ids");
        }
    }

    private void ClearRingsUI()
    {
        foreach (Transform c in wornTransform) Destroy(c.gameObject);
        if (InScene == InScene.Battle) return; // Battle scene does not have stored rings
        foreach (Transform c in storedTransform) Destroy(c.gameObject);
    }

    private void CreateRingButton(Ring ring, Transform parent, int index = -1)
    {
        var btn = Instantiate(ringButton, parent);
        btn.Ring = ring;
        btn.OnClickAction = RingButtonClicked;
        btn.Index = index;
    }

    private void LoadCharacterInfo(Character character)
    {
        characterInfoPanel.gameObject.SetActive(true);
        characterInfoPanel.SetCharacter(character);
        SetFocusAndDescription(null, true);
        ClearRingsUI();
        wornTransform.gameObject.SetActive(true);
        foreach (Ring ring in character.Rings)
        {
            CreateRingButton(ring, wornTransform);
        }
    }

    private void RingButtonClicked(RingButton ringButton)
    {
        if (InScene == InScene.Battle)
        {
            SetFocusAndDescription(ringButton);
        }
        else
        {
            if (focusButton != ringButton)
            {
                if (ringButton.Ring != null)
                {
                    SetFocusAndDescription(ringButton);
                }
                else if (focusButton != null)
                {
                    // 直接用 ringButton.Index
                    TransferRing(focusButton, ringButton.Index, focusButton.transform.parent == wornTransform);
                }
                else
                {
                    SetFocusAndDescription(null, true);
                }
            }
            else
            {
                // set index according to source
                int index = -1;
                if (focusButton.transform.parent == wornTransform)
                {
                    // worn ring -> stored，直接用 focusButton.Index
                    index = focusButton.Index;
                }
                else if (focusButton.transform.parent == storedTransform)
                {
                    // stored ring -> worn，找 wornRings 第一个空槽
                    for (int i = 0; i < wornRings.Length; i++)
                    {
                        if (wornRings[i] == null)
                        {
                            index = i;
                            break;
                        }
                    }
                }
                TransferRing(focusButton, index);
            }
        }
    }

    /// <summary>
    /// 设置当前选中的戒指按钮，并将焦点图片移动到该按钮
    /// </summary>
    private void SetFocusAndDescription(RingButton ringButton, bool isHide = false)
    {
        if (isHide)
        {
            focusImage.transform.SetParent(transform, false);
            focusImage.gameObject.SetActive(false);
            ringDescriptionPanel.gameObject.SetActive(false);
        }
        else
        {
            focusImage.SetParent(ringButton.transform, false);
            focusImage.SetSiblingIndex(0);
            focusImage.gameObject.SetActive(true);
            focusButton = ringButton;
            ringDescriptionPanel.Ring = ringButton.Ring;
            ringDescriptionPanel.gameObject.SetActive(true);
        }
    }

    private void TransferRing(RingButton ringButton, int wornButtonIndex = -1, bool isChangeWithinWorn = false)
    {
        if (ringButton.transform.parent == wornTransform)
        {
            if (isChangeWithinWorn)
            {
                int id = -1;
                for (int i = 0; i < wornTransform.childCount; i++)
                {
                    if (wornTransform.GetChild(i) == ringButton.transform)
                    {
                        id = i;
                        break;
                    }
                }
                wornRings[id] = null;
                wornRings[wornButtonIndex] = ringButton.Ring;
            }
            else
            {
                wornRings[wornButtonIndex] = null;
                storedRings.Add(ringButton.Ring);
            }
        }
        else
        {
            storedRings.Remove(ringButton.Ring);
            wornRings[wornButtonIndex] = ringButton.Ring;
        }
        SetFocusAndDescription(null, true);
        SaveRings();
        LoadRings();
    }

    public void ExitOnClick()
    {
        gameObject.SetActive(false);
        focusButton = null;
        focusImage.gameObject.SetActive(false);
    }
}
