using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] private Transform contentRoot; // 用于放置状态图标的父物体
    [SerializeField] private GameObject statusButtonTemplate; // 状态图标预制体

    public List<Status> Statuses;

    private void Awake()
    {
        statusButtonTemplate.SetActive(false);
    }

    public void UpdateContent()
    {
        // 清空旧的图标
        if (contentRoot == null || statusButtonTemplate == null) return;
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        // 为每个状态生成图标
        statusButtonTemplate.SetActive(true);
        foreach (var status in Statuses)
        {
            StatusButton icon = Instantiate(statusButtonTemplate, contentRoot).GetComponent<StatusButton>();
            icon.Setup(status);
            // 可在此设置icon的显示内容，如icon.GetComponent<StatusIcon>().Setup(status);
        }
        statusButtonTemplate.SetActive(false);
    }
}
