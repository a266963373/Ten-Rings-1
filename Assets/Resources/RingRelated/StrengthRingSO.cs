using UnityEngine;

[CreateAssetMenu(menuName = "Rings/StrengthRingSO")]
public class StrengthRingSO : RingSO
{
    protected override void InitStatModifiers()
    {
        StatModifiers.Add(new()
        {
            StatType = StatType.STR,
            Value = Power,
            Source = name
        });
    }
}
