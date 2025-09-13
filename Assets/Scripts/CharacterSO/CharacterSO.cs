using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public string CharacterName;
    public List<StatEntry> StatEntries;
    //public List<RingSO> Rings;
    public int[] RingIds = new int[10];

    public virtual CharacterStats GetStats() {  return new(StatEntries); }
    public virtual Ring[] GetRings()
    {
        Ring[] result = new Ring[10];
        for (int i = 0; i < result.Length; i++)
        {
            if (i < RingIds.Length)
                result[i] = RingLibrary.I.GetRingById(RingIds[i]);
            else
                result[i] = null;
        }
        return result;
    }
}

[Serializable]
public class StatEntry
{
    public StatType Type;
    public int Value;
}
