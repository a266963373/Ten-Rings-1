using UnityEngine;

[CreateAssetMenu(menuName = "Characters/HumanSO")]
public class HumanSO : CharacterSO
{
    public override CharacterStats GetStats()
    {
        CharacterSO templateSO = Resources.Load<CharacterSO>("ScriptableObjects/Characters/HumanSO");
        return new(templateSO.StatEntries);
    }
}
