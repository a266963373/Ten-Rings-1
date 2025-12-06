using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/FieldActionSO")]
public class FieldActionSO : BattleActionSO
{
    public FieldSO Field;
    public int FieldDuration = 5;

    public override BattleAction GetAction(Character actor, Character target)
    {
        var baseAction = base.GetAction(actor, target);

        Field runtimeField = null;

        if (Field != null)
        {
            runtimeField = Field.GetField();
            runtimeField.Duration = FieldDuration;
        }
        if (runtimeField != null)
        {
            baseAction.Field = runtimeField;
        }
        return baseAction;
    }
}
