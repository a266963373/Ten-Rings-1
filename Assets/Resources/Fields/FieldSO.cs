using UnityEngine;

public enum FieldType
{
    Terrain,
    Weather,
}

[CreateAssetMenu(menuName = "Field/FieldSO")]
public class FieldSO : ScriptableObject
{
    public int Id;
    public string Name;
    public int Power = 10;
    public FieldType Type;

    public Field GetField()
    {
        Field field = new()
        {
            Id = Id,
            Name = Name,
            Power = Power,
            Type = Type,
        };
        InitField(field);
        return field;
    }

    protected virtual void InitField(Field field) { }
}
