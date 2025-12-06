using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDescriptionPanel : MonoBehaviour
{
    [SerializeField] GameObject ringDescriptionGO;
    [SerializeField] RingDescriptionText ringDescriptionText;
    [SerializeField] RingStatText ringStatText;

    [SerializeField] GameObject actionDescriptionGO;
    [SerializeField] ActionDescriptionText actionDescriptionText;
    [SerializeField] ActionStatText actionStatText;

    private Ring ring;
    public Ring Ring
    {
        get { return ring; }
        set
        {
            ring = value;
            if (ring == null)
            {
                ringDescriptionGO.SetActive(false);
                actionDescriptionGO.SetActive(false);
                return;
            }
            ShowRingDescription();
            ShowActionDescription();
        }
    }

    private void Awake()
    {
        ringDescriptionGO.SetActive(true);
        actionDescriptionGO.SetActive(false);
    }

    private void ShowRingDescription()
    {
        ringDescriptionGO.SetActive(true);
        ringDescriptionText.Ring = ring;
        ringStatText.Ring = ring;
    }

    private void ShowActionDescription()
    {
        if (ring.GrantedActions.Count == 0)
        {
            actionDescriptionGO.SetActive(false);
        }
        else
        {
            actionDescriptionText.BattleAction = ring.GrantedActions[0];
            actionStatText.BattleAction = ring.GrantedActions[0];
            actionDescriptionGO.SetActive(true);
        }
    }
}
