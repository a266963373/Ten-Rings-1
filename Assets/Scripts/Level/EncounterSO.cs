using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level and Encounter/EncounterSO")]
public class EncounterSO : ScriptableObject
{
    public string Name;  // Key to "A slime wants to teach you fight."
    public List<CharacterSO> Characters;
    public int Gold;
}
