using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/PoisonedSO")]
public class PoisonedSO : StatusSO
{
    public override void OnWorldTurnEffect(Character c)
    {
        int stack = (int)Mathf.Max(Stack, 1);
        int last = 11 - stack;
        Power = stack * (10 + last) / 2;
        Power = Mathf.Min(Power, 50);

        Damage damage = new()
        {
            Value = Power,
            Scale = StatType.NON,
            Range = DamageRange.Indirect,
            Element = DamageElement.Poison,
        };

        BattleAction battleAction = new()
        {
            Damage = damage,
            Target = c,
        };
        battleAction.Resolve();
    }
}
