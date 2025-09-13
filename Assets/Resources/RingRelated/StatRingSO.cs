using UnityEngine;

[CreateAssetMenu(menuName = "Rings/StatRingSO")]
public class StatRingSO : RingSO
{
    public StatType StatType = StatType.STR;
    protected override void InitRing(Ring ring)
    {
        ring.StatModifiers.Add(new()
        {
            StatType = StatType,
            Value = Power,
            Source = name
        });
    }
}
