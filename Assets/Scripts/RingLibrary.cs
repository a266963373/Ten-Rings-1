using System.Collections.Generic;
using UnityEngine;

public class RingLibrary : MonoBehaviour
{
    public static RingLibrary I { get; private set; }

    private Dictionary<int, RingSO> ringTemplates = new();

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
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
        foreach (var ring in allRings)
        {
            ringTemplates[ring.Id] = ring; // 保存原始引用，不做 Instantiate
        }
    }

    /// <summary>
    /// 获取指定 ID 的 ring 副本，可安全修改
    /// </summary>
    public RingSO GetRingById(int id)
    {
        if (ringTemplates.TryGetValue(id, out var template))
            return ScriptableObject.Instantiate(template); // 返回副本

        Debug.LogWarning($"Ring with id {id} not found.");
        return null;
    }

    public List<RingSO> GetRandomRings(int count, int minId = int.MinValue, int maxId = int.MaxValue)
    {
        List<int> validIds = new();
        foreach (int id in ringTemplates.Keys)
        {
            if (id >= minId && id <= maxId)
                validIds.Add(id);
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

        List<RingSO> results = new();
        for (int i = 0; i < count; i++)
        {
            int id = validIds[i];
            results.Add(ScriptableObject.Instantiate(ringTemplates[id]));
        }

        return results;
    }

}
