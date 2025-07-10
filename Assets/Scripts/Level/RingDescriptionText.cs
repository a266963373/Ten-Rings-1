using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class RingDescriptionText : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    [SerializeField] bool isInLevelScene;
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
            localizeStringEvent.StringReference.Add("Power", new IntVariable { Value = Power });
            Show();
        }
    }

    private void Start()
    {
        Gold = BattleSession.Encounter.Gold;
        localizeStringEvent.StringReference.Add("Gold", new IntVariable { Value = Gold });
        if (isInLevelScene)
            localizeStringEvent.StringReference.SetReference("UI Text", "AfterWinBattle");
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
