using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/DefendingStatusSO")]
public class DefendingStatusSO : StatusSO
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
            Value = Power
        });

        status.StatModifiers.Add(new()
        {
            StatType = StatType.MPR,
            Value = Power
        });
    }
}
