using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public string CharacterName;
    public List<StatEntry> StatEntries;

    public virtual CharacterStats GetStats() {  return new(StatEntries); }
}

[Serializable]
public class StatEntry
{
    public StatType Type;
    public int Value;
}
