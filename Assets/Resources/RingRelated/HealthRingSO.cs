using UnityEngine;

[CreateAssetMenu(menuName = "Rings/HealthRingSO")]
public class HealthRingSO : RingSO
{
    protected override void InitStatModifiers()
    {
        StatModifiers.Add(new()
        {
            StatType = StatType.MHP,
            Value = Power,
            Source = name
        });
    }
}
