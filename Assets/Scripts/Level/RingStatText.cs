using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class RingStatText : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    private Ring ring;

    public Ring Ring
    {
        get { return ring; }
        set
        {
            ring = value;
            if (ring.RequiredRingId != -1)
            {
                string ringName = RingLibrary.I.GetRingById(ring.RequiredRingId).Name;
                ringName = new LocalizedString("Ring Name", ringName).GetLocalizedString();

                localizeStringEvent.StringReference.Remove("Requirement");
                localizeStringEvent.StringReference.Add("Requirement", new StringVariable { Value = ringName });

                localizeStringEvent.StringReference.SetReference("Misc", "Ring Requirement");
                localizeStringEvent.RefreshString();
                gameObject.SetActive(true);
            }
            else if (ring.Type == RingType.Skill)
            {
                // 获取等级和元素
                int level = ring.ActionLevel;
                string elementString = new LocalizedString("Damage Element", ring.Element.ToString()).GetLocalizedString();

                // 设置本地化参数
                localizeStringEvent.StringReference.Remove("Level");
                localizeStringEvent.StringReference.Remove("Element");
                localizeStringEvent.StringReference.Add("Level", new IntVariable { Value = level });
                localizeStringEvent.StringReference.Add("Element", new StringVariable { Value = elementString });

                localizeStringEvent.StringReference.SetReference("Misc", "Ring Stat");
                localizeStringEvent.RefreshString();
                gameObject.SetActive(true);
            }
            else if (ring.Type == RingType.Protocol)
            {
                localizeStringEvent.StringReference.SetReference("Misc", "Ring Protocol Requirement");
                localizeStringEvent.RefreshString();
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}