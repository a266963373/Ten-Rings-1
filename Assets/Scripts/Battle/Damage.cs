using UnityEngine;

public enum DamageRange
{
    Melee,
    Ranged,
    Global,  // for "insult skill"
    Indirect, // for "poison skill"
}

public enum DamageForm
{
    Sharp,   // 锐利
    Blunt,   // 钝打
    Thermal,  // 温度（泛指火热或寒冷）
    Radiant, // 辐射
}

public enum DamageElement
{
    Bio,
    Fire,
    Water,
    Grass,
    Earth,
    Electric,
    Poison,
    Metal,
    Light,
    Dark,
}

public enum DamageTargetType
{
    Single,
    Area
}

public enum DamageProperty
{
    HP,
    MP
}

[System.Serializable]
public class Damage
{
    public int Value = 0;
    public int Reduction = 0;   // reduce damage
    public int Shield = 0;  // reduce "reduction"

    //public StatType Scale = StatType.MND;
    public DamageRange Range = DamageRange.Ranged;
    public DamageForm Form = DamageForm.Blunt;
    public DamageElement Element = DamageElement.Bio;
    public DamageTargetType TargetType = DamageTargetType.Single;
    public DamageProperty Property = DamageProperty.HP;

    public Damage() { }
    public bool IsHealing => Value < 0;

    public Damage(Damage other)
    {
        Value = other.Value;
        //Scale = other.Scale;
        Range = other.Range;
        Form = other.Form;
        Element = other.Element;
        TargetType = other.TargetType;
        Property = other.Property;
    }

    public override string ToString()
    {
        return $"{Value} {Range} {Form} {Element}";
    }
    public void FinalizeValue()
    {
        Value = Value - Mathf.Max(0, Reduction - Shield);
    }
}
