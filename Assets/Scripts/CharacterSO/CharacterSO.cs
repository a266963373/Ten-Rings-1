using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public string CharacterName;
    public string Template;
    public List<StatEntry> StatEntries;
    //public List<RingSO> Rings;
    public int[] RingIds = new int[10];

    public virtual CharacterStats GetStats()
    {
        List<StatEntry> combinedStats = new ();
        
        // 先尝试从模板加载 StatEntries
        if (!string.IsNullOrEmpty(Template))
        {
            CharacterTemplateSO templateSO = Resources.Load<CharacterTemplateSO>(
                $"ScriptableObjects/Characters/Templates/{Template}CharacterTemplateSO");
            if (templateSO != null && templateSO.StatEntries != null)
            {
                combinedStats.AddRange(templateSO.StatEntries);
            }
        }
        
        // 再添加自己的 StatEntries
        if (StatEntries != null)
        {
            combinedStats.AddRange(StatEntries);
        }
        
        return new(combinedStats);
    }

    public virtual Ring[] GetRings()
    {
        Ring[] result = new Ring[10];
        int currentIndex = 0;

        // 先尝试从模板加载 RingIds
        if (!string.IsNullOrEmpty(Template))
        {
            CharacterTemplateSO templateSO = Resources.Load<CharacterTemplateSO>(
                $"ScriptableObjects/Characters/Templates/{Template}CharacterTemplateSO");
            if (templateSO != null && templateSO.RingIds != null)
            {
                for (int i = 0; i < templateSO.RingIds.Length && i < 10; i++)
                {
                    if (templateSO.RingIds[i] != 0)
                    {
                        result[i] = RingLibrary.I.GetRingById(templateSO.RingIds[i]);
                        currentIndex = i + 1;
                    }
                }
            }
        }

        // 继续添加自己的 RingIds
        if (RingIds != null)
        {
            for (int i = 0; i < RingIds.Length && currentIndex < 10; i++)
            {
                if (RingIds[i] != 0)
                {
                    result[currentIndex] = RingLibrary.I.GetRingById(RingIds[i]);
                    currentIndex++;
                }
            }
        }

        return result;
    }
}

[Serializable]
public class StatEntry
{
    public StatType Type;
    public int Value;
}
