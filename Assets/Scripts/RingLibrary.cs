using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RingLibrary : MonoBehaviour
{
    public static RingLibrary I { get; private set; }

    private Dictionary<int, RingSO> ringTemplates = new();
    public int RingCount = 0;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            //DontDestroyOnLoad(gameObject);
            LoadAllRings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllRings()
    {
        RingSO[] allRings = Resources.LoadAll<RingSO>("RingRelated");
        RingCount = allRings.Length;
        foreach (var ring in allRings)
        {
            ringTemplates[ring.Id] = ring; // 保存原始引用，不做 Instantiate
        }
    }

    /// <summary>
    /// 获取指定 ID 的 ring 副本，可安全修改
    /// </summary>
    public Ring GetRingById(int id)
    {
        if (id == 0) return null; // 0 ID 通常表示无戒指
        if (ringTemplates.TryGetValue(id, out var template))
            return template.GetRing(); // 返回副本

        Debug.LogWarning($"Ring with id {id} not found.");
        return null;
    }

    public List<Ring> GetRandomRings(int count, int minId = int.MinValue, int maxId = int.MaxValue, List<int> idBlacklist = null, 
        bool isSkillRing = false)
    {
        List<int> validIds = new();
        foreach (int id in ringTemplates.Keys)
        {
            if (id >= minId && id <= maxId && (idBlacklist == null || !idBlacklist.Contains(id)))
            {
                Ring r = GetRingById(id);
                if (isSkillRing && (r.Type != RingType.Skill || r.GrantedActions.Count == 0)) continue;
                validIds.Add(id);
            }
        }

        if (validIds.Count == 0)
        {
            Debug.LogWarning($"No rings found in range {minId} to {maxId}");
            return new();
        }

        if (count > validIds.Count)
        {
            Debug.LogWarning($"Requested {count} rings but only {validIds.Count} available. Returning all.");
            count = validIds.Count;
        }

        // 洗牌
        for (int i = validIds.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (validIds[i], validIds[j]) = (validIds[j], validIds[i]);
        }

        List<Ring> results = new();
        for (int i = 0; i < count; i++)
        {
            results.Add(GetRingById(validIds[i]));
        }

        return results;
    }

    public void FillRandomIds(List<int> targetList, int targetCount)
    {
        int missingCount = targetCount - targetList.Count;
        if (missingCount <= 0)
            return;

        List<int> validIds = new();
        foreach (int id in ringTemplates.Keys)
        {
            if (!targetList.Contains(id))
                validIds.Add(id);
        }

        if (validIds.Count < missingCount)
        {
            missingCount = validIds.Count;
        }

        // Fisher-Yates 洗牌
        for (int i = validIds.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (validIds[i], validIds[j]) = (validIds[j], validIds[i]);
        }

        targetList.AddRange(validIds.Take(missingCount));
    }

}
