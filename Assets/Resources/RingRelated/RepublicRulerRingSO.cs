using UnityEngine;

public enum RepublicClass
{
    Ruler,
    Warrior,
    Producer,
}

[CreateAssetMenu(menuName = "Rings/RepublicClassRingSO")]
public class RepublicClassRingSO : RingSO
{
    public RepublicClass RepublicClass;

    protected override void InitRing(Ring ring)
    {
        ring.TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnBeforeAction,
            Effect = (BattleAction ba) =>
            {
                if ((RepublicClass == RepublicClass.Ruler && ba.Target == null)
                    || (RepublicClass == RepublicClass.Warrior && ba.Target.IsPlayerSide != ba.Actor.IsPlayerSide)
                    || (RepublicClass == RepublicClass.Producer && ba.Target.IsPlayerSide == ba.Actor.IsPlayerSide))
                {
                    ba.Power += ring.Power;
                }
            }
        });
    }
}
