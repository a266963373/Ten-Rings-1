using UnityEngine;

[CreateAssetMenu(menuName = "Rings/HealthRingSO")]
public class HealthRingSO : RingSO
{
    protected override void InitStatModifiers()
    {
        StatModifiers.Add(new(
            value: power,
            statType: StatType.MHP
            ));
    }
}
