using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/EmpowermentRingSO")]
public class EmpowermentRingSO : RingSO
{
    public Action<BattleAction> Effect;
    protected override void InitRing(Ring ring)
    {
        ring.Type = RingType.Empowerment;
        ring.Empowerment = new()
        {
            Effect = Effect
        };
    }
}
