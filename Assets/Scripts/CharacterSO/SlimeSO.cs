using UnityEngine;

[CreateAssetMenu(menuName = "Character/SlimeSO")]
public class SlimeSO : CharacterSO
{
    public override CharacterStats GetStats()
    {
        CharacterSO templateSO = Resources.Load<CharacterSO>("ScriptableObjects/Characters/SlimeSO");
        return new(templateSO.StatEntries);
    }
}
