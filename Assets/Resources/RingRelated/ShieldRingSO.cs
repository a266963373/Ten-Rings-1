using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ShieldRingSO")]
public class ShieldRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        TriggerEffect trigFx = new()
        {
            Trigger = TriggerType.OnBlocking,
            Effect = Effect
        };
        ring.TriggerEffects.Add(trigFx);
    }

    private void Effect(BattleAction context)
    {
        context.Damage.Value -= Power;
    }
}
