using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
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

[CreateAssetMenu(menuName = "Character/HumanSO")]
public class HumanSO : CharacterSO
{
    public override CharacterStats GetStats()
    {
        CharacterSO templateSO = Resources.Load<CharacterSO>("ScriptableObjects/Characters/HumanSO");
        return new(templateSO.StatEntries);
    }
}
