using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class StatusSystem
{
    public List<Status> Statuses { get; } = new(); // for status effects
    public event Action OnStatusChanged;
    public Character Owner;  // The character that owns this status system

    public void OnTurn(StatusDecayTrigger statusDecayTrigger)
    {
        // 记录需要移除的状态
        List<Status> toRemove = new();

        foreach (var status in Statuses)
        {
            status.OnTurn(statusDecayTrigger, Owner);

            // 自动减少持续时间
            if (status.DecayTrigger == statusDecayTrigger && status.Stack > 0)
            {
                if (status.DecayStatType == StatType.NON)
                {
                    status.Stack--;
                } else
                {
                    float decayAmount = Owner.GetStat(status.DecayStatType) / 100f;
                    status.Stack -= decayAmount;
                }
            }

            // 持续时间为0则移除
            if (status.Stack == 0)
                toRemove.Add(status);
        }

        foreach (var status in toRemove)
        {
            Statuses.Remove(status);
        }

        //if (toRemove.Count > 0)
        OnStatusChanged?.Invoke();
    }

    public void AddStatus(Status status)
    {
        if (Owner.IsDead) return;
        var existing = Statuses.Find(s => s.Id == status.Id);

        if (existing == null)
        {
            Statuses.Add(status);
        }
        else if (status.IsStackable)
        {
            // 叠加Power和Duration
            existing.Stack += status.Stack;
        }
        else
        {
            existing.Stack = Mathf.Max(existing.Stack, status.Stack);
        }

        OnStatusChanged?.Invoke();
    }

    public void RemoveStatus(Status status)
    {
        var existing = Statuses.Find(s => s.Id == status.Id);
        if (existing != null)
        {
            Statuses.Remove(existing);
            OnStatusChanged?.Invoke();
        }
    }
}
