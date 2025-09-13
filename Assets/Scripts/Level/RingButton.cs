using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization.Components;

public class RingButton : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    public Action<RingButton> OnClickAction;
    public int Index { get; set; }
    private Ring ring;
    public Ring Ring
    {
        get { return ring; }
        set
        {
            ring = value;
            if (ring != null)
                localizeStringEvent.StringReference.SetReference("Ring Name", ring.Name);
        }
    }

    public void OnClick()
    {
        OnClickAction?.Invoke(this);
    }

}
