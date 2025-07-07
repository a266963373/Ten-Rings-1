using UnityEngine;

[CreateAssetMenu(menuName = "Rings/StrengthRingSO")]
public class StrengthRingSO : RingSO
{
    protected override void InitStatModifiers()
    {
        StatModifiers.Add(new()
        {
            Value = power,
            StatType = StatType.STR
        });
    }
}
