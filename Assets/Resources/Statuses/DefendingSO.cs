using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/DefendingSO")]
public class DefendingSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.StatModifiers.Add(new()
        {
            StatType = StatType.BLK,
            Value = Power
        });

        status.StatModifiers.Add(new()
        {
            StatType = StatType.HPR,
            Value = Power / 10
        });

        status.StatModifiers.Add(new()
        {
            StatType = StatType.MPR,
            Value = Power / 10
        });
    }
}
