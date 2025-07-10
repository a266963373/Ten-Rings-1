using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization.Components;

public class RingButton : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    public Action<RingButton> OnClickAction;
    private RingSO ring;
    public RingSO Ring
    {
        get { return ring; }
        set
        {
            ring = value;
            localizeStringEvent.StringReference.SetReference("Ring Name", ring.Name);
        }
    }

    public void OnClick()
    {
        OnClickAction?.Invoke(this);
    }

}
