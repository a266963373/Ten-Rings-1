using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/StatusSO")]
public class StatusSO : ScriptableObject
{
    public int Id;
    public string Name;     // used for index
    public int Power = 5;    // for status' effect strength
    public float Stack = 0;
    public bool IsBuff = false;               // 是否为正面效果
    public bool IsStackable = false;
    public List<StatModifier> StatModifiers = new();
    public virtual void OnWorldTurnEffect(Character c) { }
}
