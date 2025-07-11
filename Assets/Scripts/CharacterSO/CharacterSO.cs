using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public string CharacterName;
    public List<StatEntry> StatEntries;
    //public List<RingSO> Rings;
    public List<int> RingIds = new();

    public virtual CharacterStats GetStats() {  return new(StatEntries); }
    public virtual List<RingSO> GetRings()
    {
        List<RingSO> copies = new();
        foreach (var ringId in RingIds)
        {
            copies.Add(RingLibrary.I.GetRingById(ringId));
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
