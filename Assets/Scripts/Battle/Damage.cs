using UnityEngine;

public enum DamageForm
{
    Sharp,   // 锐利
    Blunt,   // 钝打
    Thermal  // 温度（泛指火热或寒冷）
}

public enum DamageElement
{
    Bio,
    Fire,
    Water,
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
    public int Value;

    public StatType Scale;
    public DamageForm Form;
    public DamageElement Element;
    public DamageTargetType TargetType;
    public DamageProperty Property;

    public Damage(
        int value,
        StatType scale = StatType.MND,
        DamageForm form = DamageForm.Blunt,
        DamageElement element = DamageElement.Bio,
        DamageTargetType targetType = DamageTargetType.Single,
        DamageProperty property = DamageProperty.HP)
    {
        Value = value;
        Scale = scale;
        Form = form;
        Element = element;
        TargetType = targetType;
        Property = property;
    }

    public Damage(Damage other)
    {
        Value = other.Value;
        Scale = other.Scale;
        Form = other.Form;
        Element = other.Element;
        TargetType = other.TargetType;
        Property = other.Property;
    }

    public override string ToString()
    {
        return $"{Value} {Form} {Element} damage to {Property} ({TargetType})";
    }
}
