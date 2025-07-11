using UnityEngine;

[CreateAssetMenu(menuName = "Characters/SlimeSO")]
public class SlimeSO : CharacterSO
{
    public override CharacterStats GetStats()
    {
        CharacterSO templateSO = Resources.Load<CharacterSO>("ScriptableObjects/Characters/SlimeSO");
        return new(templateSO.StatEntries);
    }
}
