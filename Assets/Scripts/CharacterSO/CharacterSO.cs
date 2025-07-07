using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public string CharacterName;
    public List<StatEntry> StatEntries;
    public List<RingSO> Rings;

    public virtual CharacterStats GetStats() {  return new(StatEntries); }
    public virtual List<RingSO> GetRings()
    {
        List<RingSO> copies = new();
        foreach (var ring in Rings)
        {
            var ringCopy = Instantiate(ring);
            copies.Add(ringCopy);
        }
        return copies;
    }
}

[Serializable]
public class StatEntry
{
    public StatType Type;
    public int Value;
}
