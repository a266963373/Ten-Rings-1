using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/PoisonedSO")]
public class PoisonedSO : StatusSO
{
    protected override void InitStatus(Status status)
    {
        status.OnWorldTurnEffect = () =>
        {
            int stack = status.EffectiveStack;
            int last = status.Power + 1 - stack;
            int dmg = stack * (status.Power + last) / 2;
            dmg = Mathf.Min(dmg, 50);

            Damage damage = new()
            {
                Value = dmg,
                Range = DamageRange.Indirect,
                Element = DamageElement.Poison,
            };

            BattleAction battleAction = new()
            {
                Name = "Poisoned",
                Damage = damage,
                Target = status.Bearer,
            };
            ActionResolver.I.Resolve(battleAction, true);
        };
    }
}
