using UnityEngine;
public enum DamageForm
{
    Sharp,   // 锐利
    Blunt,   // 钝打
    Thermal,  // 温度（泛指火热或寒冷）
    Radiant, // 辐射
    Internal, // 内部
}

public enum Element
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
    Ice,
    Air,
    None,
}

//public enum DamageTargetType
//{
//    Single,
//    Area
//}

[System.Serializable]
public class Damage
{
    public int Value = 0;
    public int Reduction = 0;   // reduce damage
    public int Shield = 0;  // reduce "reduction"
    public int Power = 100; // effectiveness, eg. remnant flame

    //public StatType Scale = StatType.MND;
    public DamageForm Form = DamageForm.Blunt;
    public Element Element = Element.Bio;
    //public DamageTargetType TargetType = DamageTargetType.Single;
    public StatType Property = StatType.HP;

    public Damage() { }
    public bool IsHealing => Value < 0;

    public Damage(Damage other)
    {
        Value = other.Value;
        //Scale = other.Scale;
        Form = other.Form;
        Element = other.Element;
        //TargetType = other.TargetType;
        Property = other.Property;

        if (Property == StatType.NON)
        {
            Property = StatType.HP;
        }
    }

    public override string ToString()
    {
        return $"{Value} {Form} {Element}";
    }
    public void FinalizeValue()
    {
        Value = Mathf.RoundToInt(Value * Power / 100f) - Mathf.Max(0, Reduction - Shield);
    }
}
