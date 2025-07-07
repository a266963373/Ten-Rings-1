using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level and Encounter/LevelSO")]
public class LevelSO : ScriptableObject
{
    public string Name;
    public List<EncounterSO> Encounters;
}
