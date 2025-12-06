using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting.FullSerializer;

public class Ring
{
    public int Id;
    public string Name;
    public int Power;
    public int Power2;
    public RingType Type;
    public int Count = 0;   // for some rings like hammer tempered
    public bool Enabled = true;

    public bool IsRequirementMet = false;
    // skill ring add skill requirements
    public int ActionLevel = 0;
    public Element Element; // for skill and form ring so far

    // some rings requires "form of element"
    public int RequiredRingId = -1;

    public List<StatModifier> StatModifiers = new();
    public List<TriggerEffect> TriggerEffects = new();
    public List<BattleActionSO> GrantedActions = new();
    public Func<Damage, Damage> DealDamageModifier;
    public Func<Damage, Damage> TakeDamageModifier;
    public List<RingDynamicStatMod> DynamicStatMods = new();
    public Empowerment Empowerment; // for action
    public Enchantment Enchantment; // for weapon

    public bool IRF(Character c) => IsRequirementFulfilled(c);
    private bool IsRequirementFulfilled(Character c)
    {
        IsRequirementMet = false;
        if (!Enabled)
        {
            return false;
        }

        if (RequiredRingId != -1)
        {
            IsRequirementMet = false;

            for (int i = 0; c.Rings[i] != this; i++)
            {
                var ring = c.Rings[i];
                if (ring != null && ring.Id == RequiredRingId)
                {
                    IsRequirementMet = true;
                    break;
                }
            }
            if (!IsRequirementMet)
            {
                return false;
            }
        }

        if (ActionLevel > 1)
        {
            IsRequirementMet = false;

            // check add action requirements
            for (int i = 0; c.Rings[i] != this; i++)
            {
                if (c.Rings[i].Element == Element
                    && c.Rings[i].ActionLevel == ActionLevel - 1
                    && c.Rings[i].IsRequirementMet)
                {
                    IsRequirementMet = true;
                    break;
                }
            }
            if (!IsRequirementMet)
            {
                return false;
            }
        }

        if (Type == RingType.Protocol)
        {
            IsRequirementMet = false;

            if (!IsWholeTeamEquipped(c))
            {
                return false;
            }
        }

        IsRequirementMet = true;
        return true;
    }

    private bool IsWholeTeamEquipped(Character character)
    {
        if (character == null) return false;
        foreach (var teammate in character.Teammates)
        {
            bool hasProtocolRing = false;
            foreach (var ring in teammate.Rings)
            {
                if (ring != null && ring.Id == this.Id)
                {
                    hasProtocolRing = true;
                    break;
                }
            }
            if (!hasProtocolRing)
            {
                return false;
            }
        }
        return true;
    }
}
