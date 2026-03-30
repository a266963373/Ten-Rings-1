using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/TheRepublicRingSO")]
public class TheRepublicRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        var negativeStatMod = new StatModifier()
        {
            StatType = StatType.ALL,
            Value = Power2
        };

        var dynamicStatMod = new RingDynamicStatMod()
        {
            CheckStatType = StatType.ALL,
            Updator = (Character c) =>
            {
                ring.Enabled = false;
                ring.StatModifiers.Clear();
                ring.StatModifiers.Add(new StatModifier()
                {
                    StatType = GetMaxType(c),
                    Value = Power
                });
                ring.StatModifiers.Add(negativeStatMod);
                ring.Enabled = true;
            }
        };

        ring.DynamicStatMods.Add(dynamicStatMod);
    }

    private StatType GetMaxType(Character c)
    {
        StatType maxType = StatType.MHP;
        int maxValue = int.MinValue;
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            if (type == StatType.NON
                || type == StatType.ALL
                || type == StatType.HP
                || type == StatType.MP)
                continue;
            int value = c.GetStat(type);
            if (value > maxValue)
            {
                maxValue = value;
                maxType = type;
            }
        }
        return maxType;
    }
}
