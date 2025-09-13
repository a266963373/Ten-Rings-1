using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/StatModStatusSO")]
public class StatModStatusSO : StatusSO
{
    public StatType AffectStatType;

    protected override void InitStatus(Status status)
    {
        status.DynamicStatMods.Add(new()
        {
            CheckStatType = AffectStatType,
            Updator = () =>
            {
                status.StatModifiers.Clear();
                status.StatModifiers.Add(new()
                {
                    StatType = AffectStatType,
                    Value = EffectValue(status.EffectiveStack) * (status.IsBuff ? 1 : -1)
                });
            }
        });
    }
}
