using System.Collections.Generic;

public class Field
{
    public int Id;
    public string Name;
    public int Power;
    public float Duration = 5;
    public FieldType Type;

    public List<TriggerEffect> TriggerEffects = new();
}
