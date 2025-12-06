using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/BurningStatusSO")]
public class BurningStatusSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect
        {
            Timing = TimingType.OnWorldTurn,
            EffectOnTime = () =>
            {
                Damage damage = new()
                {
                    Value = status.CommonEffectValue(false),
                    Form = DamageForm.Thermal,
                    Element = Element.Fire,
                };

                BattleAction battleAction = new()
                {
                    Name = Name,
                    Target = status.Bearer,
                    Damage = damage,
                    Range = RangeType.Indirect,
                    Element = Element.Fire,
                };
                ActionResolver.I.Resolve(battleAction, true);
            }
        });

        status.OnApplyEffect = () =>
        {
            var ss = status.Bearer.StatusSystem;
            var wetStatus = ss.GetStatusByName("Wet");
            if (wetStatus != null)
            {
                var deductStack = Mathf.Min(wetStatus.Stack, status.Stack);
                ss.ModifyStatus(wetStatus, -deductStack);
                ss.ModifyStatus(status, -deductStack);
            }
        };
    }
}
