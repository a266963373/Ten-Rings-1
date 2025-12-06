using UnityEngine;

[CreateAssetMenu(menuName = "Field/RainFieldSO")]
public class RainFieldSO : FieldSO
{
    protected override void InitField(Field field)
    {
        field.TriggerEffects.Add(new TriggerEffect
        {
            Timing = TimingType.OnWorldTurn,
            EffectOnTime = () =>
            {
                foreach (var c in BattleSystem.I.Characters)
                {
                    ActionResolver.I.ApplyStatusByName(null, c, "Wet", Power);
                }
            }
        });
    }
}
