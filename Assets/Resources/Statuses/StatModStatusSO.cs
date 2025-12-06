using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/StatModStatusSO")]
public class StatModStatusSO : StatusSO
{
    public StatType AffectStatType1 = StatType.NON;
    public bool IsPositive1 = true;
    public StatType AffectStatType2 = StatType.NON;
    public bool IsPositive2 = false;

    protected override void InitStatus(Status status)
    {
        status.DynamicStatMods.Add(new()
        {
            CheckStatType = AffectStatType1,
            Updator = () =>
            {
                status.StatModifiers.Clear();
                status.StatModifiers.Add(new()
                {
                    StatType = AffectStatType1,
                    Value = status.CommonEffectValue() * (IsPositive1 ? 1 : -1)
                });
            }
        });

        if (AffectStatType2 != StatType.NON)
        {
            status.DynamicStatMods.Add(new()
            {
                CheckStatType = AffectStatType2,
                Updator = () =>
                {
                    status.StatModifiers.Add(new()
                    {
                        StatType = AffectStatType2,
                        Value = status.CommonEffectValue() * (IsPositive2 ? 1 : -1)
                    });
                }
            });
        }
    }
}
