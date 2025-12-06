using UnityEngine;

[CreateAssetMenu(menuName = "Characters/PlantSO")]
public class PlantSO : CharacterSO
{
    public override CharacterStats GetStats()
    {
        CharacterSO templateSO = Resources.Load<CharacterSO>($"ScriptableObjects/Characters/{GetType().Name}");
        return new(templateSO.StatEntries);
    }
}
