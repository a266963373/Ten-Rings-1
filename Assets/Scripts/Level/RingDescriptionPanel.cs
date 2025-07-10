using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDescriptionPanel : MonoBehaviour
{
    [SerializeField] GameObject ringDescriptionGO;
    [SerializeField] RingDescriptionText ringDescriptionText;
    [SerializeField] GameObject actionDescriptionGO;
    [SerializeField] ActionDescriptionText actionDescriptionText;

    private RingSO ring;
    public RingSO Ring
    {
        get { return ring; }
        set
        {
            ring = value;
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
        ringDescriptionText.Ring = ring;
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
            actionDescriptionGO.SetActive(true);
        }
    }
}
