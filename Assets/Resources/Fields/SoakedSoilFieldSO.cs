using UnityEngine;

[CreateAssetMenu(menuName = "Field/SoakedSoilFieldSO")]
public class SoakedSoilFieldSO : FieldSO
{
    protected override void InitField(Field field)
    {
        field.TriggerEffects.Add(new TriggerEffect
        {
            IntMod = IntModType.ManaCost,
            EffectOnInt = (BattleActionSO ba) =>
            {
                if (ba.Element == Element.Water || ba.Element == Element.Earth)
                {
                    return ba.ManaCost / Power;
                }
                return ba.ManaCost;
            }
        });
    }
}
