using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Rings/BeginnersLuckRingSO")]
public class BeginnersLuckRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.DynamicStatMods.Add(new RingDynamicStatMod
        {
            CheckStatType = StatType.CRT,
            Updator = c => UpdateStatMod(ring, c, StatType.CRT)
        });
        ring.DynamicStatMods.Add(new RingDynamicStatMod
        {
            CheckStatType = StatType.EVS,
            Updator = c => UpdateStatMod(ring, c, StatType.EVS)
        });
    }

    private void UpdateStatMod(Ring ring, Character c, StatType type)
    {
        int value = Power * c.Rings.Count(r => r == null);
        // 只移除本 ring 的动态加成
        ring.StatModifiers.RemoveAll(mod => mod.Source == name && mod.StatType == type);
        ring.StatModifiers.Add(new StatModifier
        {
            StatType = type,
            Value = value,
            Source = name
        });
    }
}
