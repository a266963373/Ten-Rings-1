using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/CryingStatusSO")]
public class CryingStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnBeforeTargeted,
            Effect = (BattleAction ba) =>
            {
                ba.Actor?.Stats.TempStatMods.Add(new StatModifier
                {
                    StatType = StatType.CRT,
                    Value = -status.CommonEffectValue(),
                });
            }
        });

        status.DynamicStatMods.Add(new StatusDynamicStatMod
        {
            CheckStatType = StatType.ACC,
            Updator = () =>
            {
                status.StatModifiers.Clear();
                status.StatModifiers.Add(new StatModifier
                {
                    StatType = StatType.ACC,
                    Value = -status.CommonEffectValue(),
                });
            }
        });
    }
}
