using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/PropertyModStatusSO")]
public class PropertyModStatusSO : StatusSO
{
    public Element Element;
    public StatType DamageProperty;

    protected override void InitStatus(Status status)
    {
        status.TriggerEffects.Add(new TriggerEffect()
        {
            Timing = TimingType.OnWorldTurn,
            EffectOnTime = () =>
            {
                Damage damage = new()
                {
                    Value = status.CommonEffectValue(false),
                    Form = DamageForm.Internal,
                    Element = Element,
                    Property = DamageProperty
                };

                BattleAction battleAction = new()
                {
                    Name = status.Name,
                    Target = status.Bearer,
                    Damage = damage,
                    Range = RangeType.Indirect,
                    Element = Element,
                };
                ActionResolver.I.Resolve(battleAction, true);
            }
        });
    }
}
