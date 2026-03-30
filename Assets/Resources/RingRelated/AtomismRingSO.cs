using UnityEngine;

[CreateAssetMenu(menuName = "Rings/AtomismRingSO")]
public class AtomismRingSO : RingSO
{
    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnBeforeTakeDamage,
            Effect = (BattleAction ba) =>
            {
                int dmg = ba.Damage.Value;
                int mod = dmg % Power;
                if (mod != 0)
                {
                    if (dmg > 0)
                        ba.Damage.Value = dmg + (Power - mod);
                    else
                        ba.Damage.Value = dmg - Power - mod;
                }
            },
        });
    }
}
