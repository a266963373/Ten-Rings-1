using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level and Encounter/LevelSO")]
public class LevelSO : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite BackgroundImage; // 改为 Sprite 类型
    public Sprite BattleImage; // 改为 Sprite 类型
    public List<EncounterSO> Encounters;
}
