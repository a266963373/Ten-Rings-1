using UnityEngine;

[CreateAssetMenu(menuName = "Characters/DogSO")]
public class DogSO : CharacterSO
{
    public override CharacterStats GetStats()
    {
        CharacterSO templateSO = Resources.Load<CharacterSO>("ScriptableObjects/Characters/DogSO");
        return new(templateSO.StatEntries);
    }
}
