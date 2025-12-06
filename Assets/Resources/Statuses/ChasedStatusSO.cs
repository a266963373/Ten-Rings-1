using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/ChasedStatusSO")]
public class ChasedStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.DynamicStatMods.Add(new()
        {
            CheckStatType = StatType.BLK,
            Updator = () =>
            {
                int value = (status.Applier.GetStat(StatType.SPD)
                    - status.Bearer.GetStat(StatType.SPD)) * -2;
                status.StatModifiers.Clear();
                status.StatModifiers.Add(new()
                {
                    StatType = StatType.BLK,
                    Value = value
                });
            }
        });
    }
}
