using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] private Transform contentRoot; // 用于放置状态图标的父物体
    [SerializeField] private GameObject statusButtonPrefab; // 状态图标预制体

    public List<StatusSO> Statuses;

    public void UpdateContent()
    {
        // 清空旧的图标
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        // 为每个状态生成图标
        foreach (var status in Statuses)
        {
            var icon = Instantiate(statusButtonPrefab, contentRoot);
            // 可在此设置icon的显示内容，如icon.GetComponent<StatusIcon>().Setup(status);
        }
    }
}
