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
    public int Gold;

    private Ring ring;
    public Ring Ring
    {
        get { return ring; }
        set
        {
            ring = value;
            if (ring == null) return;
            localizeStringEvent.StringReference.Add("Power", new IntVariable { Value = ring.Power });
            localizeStringEvent.StringReference.Add("Power2", new IntVariable { Value = ring.Power2 });
            Show();
        }
    }

    private void Start()
    {
        Gold = BattleSession.Encounter.Gold;
        localizeStringEvent.StringReference.Add("Gold", new IntVariable { Value = Gold });
        if (isInLevelScene)
        {
            if (BattleSession.IsLastEncounter)
            {
                localizeStringEvent.StringReference.SetReference("UI Text", "CompletedLevel");
            }
            else
            {
                localizeStringEvent.StringReference.SetReference("UI Text", "AfterWinBattle");
            }
        }
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
