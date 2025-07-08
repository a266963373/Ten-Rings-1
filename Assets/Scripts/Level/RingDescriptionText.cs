using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class RingDescriptionText : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    // for LSE augments
    public int Power;
    public int Gold;

    private RingSO ring;
    public RingSO Ring
    {
        get { return ring; }
        set
        {
            ring = value;
            Power = ring.power;
            Show();
        }
    }

    private void Start()
    {
        Gold = BattleSession.Encounter.Gold;
    }

    private void Show()
    {
        localizeStringEvent.StringReference.SetReference("Ring Description", ring.Name);
    }

    public void GoldSelected()
    {
        localizeStringEvent.StringReference.SetReference("UI Text", "AdditionalGoldSelected");
    }

}
