using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StatusLibrary : MonoBehaviour
{
    public static StatusLibrary I { get; private set; }

    private readonly Dictionary<string, StatusSO> statusTemplates = new();

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
            LoadAllStatuses();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllStatuses()
    {
        StatusSO[] allStatuses = Resources.LoadAll<StatusSO>("Statuses");
        foreach (var status in allStatuses)
        {
            // 用 status.Name 作为 key，保存原始引用，不做 Instantiate
            statusTemplates[status.Name] = status;
        }
    }

    /// <summary>
    /// 获取指定名称的 status 副本，可安全修改
    /// </summary>
    public StatusSO GetStatusByName(string name)
    {
        if (statusTemplates.TryGetValue(name, out var template))
        {
            // 返回副本，避免直接修改原始模板
            return Instantiate(template);
        }
        return null;
    }
}
