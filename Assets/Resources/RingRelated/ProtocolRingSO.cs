using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/ProtocolRingSO")]
public class ProtocolRingSO : RingSO
{
    // related functionalities have been moved to Ring IRF
    protected override void InitRing(Ring ring)
    {
        ring.Type = RingType.Protocol;
    }

    //protected List<TriggerEffect> ProtocolEffects = new();
    //protected List<StatModifier> ProtocolStatModifiers = new();
    //protected List<RingDynamicStatMod> ProtocolDynamicStatMods = new();

    //protected override void InitRing(Ring ring)
    //{
    //    // 处理所有 ProtocolEffects
    //    foreach (var effect in ProtocolEffects)
    //    {
    //        if (effect != null)
    //        {
    //            ring.TriggerEffects.Add(new TriggerEffect()
    //            {
    //                Trigger = effect.Trigger,
    //                Effect = (BattleAction ba) =>
    //                {
    //                    var triggerer = effect.GetTriggerer(ba);
    //                    if (triggerer != null && IsWholeTeamEquipped(triggerer))
    //                    {
    //                        effect.Effect?.Invoke(ba);
    //                    }
    //                },
    //            });
    //        }
    //    }

    //    // 处理所有 ProtocolStatModifiers
    //    foreach (var statMod in ProtocolStatModifiers)
    //    {
    //        if (statMod != null)
    //        {
    //            ring.DynamicStatMods.Add(new RingDynamicStatMod()
    //            {
    //                CheckStatType = statMod.StatType,
    //                Updator = (Character c) =>
    //                {
    //                    if (IsWholeTeamEquipped(c))
    //                    {
    //                        ring.StatModifiers.Add(statMod);
    //                    }
    //                    else
    //                    {
    //                        // Remove the stat mod if the whole team is not equipped
    //                        ring.StatModifiers.Remove(statMod);
    //                    }
    //                },
    //            });
    //        }
    //    }

    //    // 处理所有 ProtocolDynamicStatMods
    //    foreach (var dynMod in ProtocolDynamicStatMods)
    //    {
    //        if (dynMod != null)
    //        {
    //            ring.DynamicStatMods.Add(new RingDynamicStatMod()
    //            {
    //                CheckStatType = dynMod.CheckStatType,
    //                Updator = (Character c) =>
    //                {
    //                    if (IsWholeTeamEquipped(c))
    //                    {
    //                        dynMod.Updator?.Invoke(c);
    //                    }
    //                    else
    //                    {
    //                        // Remove the stat mod if the whole team is not equipped
    //                        ring.StatModifiers.RemoveAll(mod => mod.Source == name && mod.StatType == dynMod.CheckStatType);
    //                    }
    //                },
    //            });
    //        }
    //    }
    //}

    //private bool IsWholeTeamEquipped(Character character)
    //{
    //    if (character == null) return false;
    //    foreach (var teammate in character.Teammates)
    //    {
    //        bool hasProtocolRing = false;
    //        foreach (var ring in teammate.Rings)
    //        {
    //            if (ring != null && ring.Id == this.Id)
    //            {
    //                hasProtocolRing = true;
    //                break;
    //            }
    //        }
    //        if (!hasProtocolRing)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
}