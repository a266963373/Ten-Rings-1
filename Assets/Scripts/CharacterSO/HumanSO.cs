using UnityEngine;

[CreateAssetMenu(menuName = "Character/HumanSO")]
public class HumanSO : CharacterSO
{
    public override CharacterStats GetStats()
    {
        CharacterSO templateSO = Resources.Load<CharacterSO>("ScriptableObjects/Characters/HumanSO");
        return new(templateSO.StatEntries);
    }
}
