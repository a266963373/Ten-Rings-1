using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StatusSystem
{
    public List<StatusSO> Statuses { get; } = new(); // for status effects
    public event Action OnStatusChanged;
    public Character Owner;  // The character that owns this status system

    public void OnWorldTurn()
    {
        // 记录需要移除的状态
        List<StatusSO> toRemove = new();

        foreach (var status in Statuses)
        {
            status.OnWorldTurnEffect(Owner);

            // 自动减少持续时间
            if (status.Stack > 0)
                status.Stack--;

            // 持续时间为0则移除
            if (status.Stack == 0)
                toRemove.Add(status);
        }

        foreach (var status in toRemove)
        {
            Statuses.Remove(status);
        }

        if (toRemove.Count > 0)
            OnStatusChanged?.Invoke();
    }

    public void AddStatus(StatusSO status)
    {
        var existing = Statuses.Find(s => s.Id == status.Id);

        if (existing != null && status.IsStackable)
        {
            // 叠加Power和Duration
            existing.Stack += status.Stack;
        }
        else
        {
            Statuses.Add(status);
        }

        OnStatusChanged?.Invoke();
    }

    public void RemoveStatus(StatusSO status)
    {
        var existing = Statuses.Find(s => s.Id == status.Id);
        if (existing != null)
        {
            Statuses.Remove(existing);
            OnStatusChanged?.Invoke();
        }
    }
}
