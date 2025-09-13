using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/FissionRingSO")]
public class FissionRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new()
        {
            Trigger = TriggerType.OnAfterDeath,
            Effect = Effect
        });
    }

    private void Effect(BattleAction context)
    {
        Character actor = context.Actor;

        // 将当前戒指从角色的 Rings 数组中移除（设为 null）
        for (int i = 0; i < actor.Rings.Length; i++)
        {
            if (actor.Rings[i]?.Id == Id)
            {
                actor.Rings[i] = null;
                break;
            }
        }

        // 3. 创建两个新角色
        Character fission1 = new(actor, 0.5f, RingInheritType.LeftHand);
        Character fission2 = new(actor, 0.5f, RingInheritType.RightHand);
        BattleLoader.I.LoadCharacter(fission1);
        BattleLoader.I.LoadCharacter(fission2);
    }
}
