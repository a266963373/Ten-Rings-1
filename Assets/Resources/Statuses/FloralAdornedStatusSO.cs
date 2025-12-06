using UnityEngine;

[CreateAssetMenu(menuName = "Status/FloralAdornedStatusSO")]
public class FloralAdornedStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnBeforeTargeted,
            Effect = (ba) =>
            {
                if (ba.Actor.IsPlayerSide != ba.Target.IsPlayerSide)
                {
                    ba.Actor.Stats.TempStatMods.Add(new StatModifier
                    {
                        StatType = StatType.CRT,
                        Value = status.CommonEffectValue(),
                    });
                }
            }
        });
    }
}
