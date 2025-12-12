using UnityEngine;

[CreateAssetMenu(menuName = "Characters/CharacterTemplateSO")]
public class CharacterTemplateSO : CharacterSO  // ethnicity, like human use human template
{
    public override CharacterStats GetStats()
    {
        CharacterSO templateSO = Resources.Load<CharacterSO>
            ($"ScriptableObjects/Characters/{Template}TemplateSO");
        return new(templateSO.StatEntries);
    }
}
