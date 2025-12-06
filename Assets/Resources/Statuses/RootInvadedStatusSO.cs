using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/RootInvadedStatusSO")]
public class RootInvadedStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect()
        {
            Timing = TimingType.OnWorldTurn,
            EffectOnTime = () =>
            {
                status.Stack += 2;
            }
        });     
        
        status.DynamicStatMods.Add(new StatusDynamicStatMod()
        {
            CheckStatType = StatType.HPR,
            Updator = () =>
            {
                StatModifier statModifier = new()
                {
                    StatType = StatType.HPR,
                    Value = - Power * status.EffectiveStack,
                };
            }
        });

        status.DynamicStatMods.Add(new StatusDynamicStatMod()
        {
            CheckStatType = StatType.MPR,
            Updator = () =>
            {
                StatModifier statModifier = new()
                {
                    StatType = StatType.MPR,
                    Value = - Power2 * status.EffectiveStack,
                };
            }
        });
    }
}
