using UnityEngine;

[CreateAssetMenu(menuName = "Field/FireWallFieldSO")]
public class FireWallFieldSO : FieldSO
{
    protected override void InitField(Field field)
    {
        field.TriggerEffects.Add(new TriggerEffect
        {
            Trigger = TriggerType.OnAfterAction,
            Effect = (action) =>
            {
                // 判断是否为近战动作
                if (!action.IsFollowUp && 
                    action.Range == RangeType.Melee && 
                    action.Actor != null && 
                    action.Target != null && 
                    action.Actor.IsPlayerSide != action.Target.IsPlayerSide)
                {
                    BattleAction newBa = new()
                    {
                        Name = "Fire Wall",
                        Target = action.Actor,
                        Range = RangeType.Indirect,
                        IsDamage = true,
                        Damage = new Damage
                        {
                            Value = Power,
                            Form = DamageForm.Thermal,
                            Element = Element.Fire,
                        },
                    };
                    ActionResolver.I.Resolve(newBa);
                }
            }
        });
    }
}
