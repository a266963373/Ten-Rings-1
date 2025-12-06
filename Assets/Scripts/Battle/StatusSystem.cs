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
    public Action<Character> OnApplierTurnBegin;

    public void Init(Character owner)
    {
        Owner = owner;
        OnApplierTurnBegin = (applier) =>
            OnTurn(TimingType.OnApplierTurnBegin, applier);
    }

    public void OnTurn(TimingType timing, Character applier=null)
    {
        foreach (var status in Statuses.ToList())
        {
            if (timing == TimingType.OnWorldTurn)
            {
                if (status.UpkeepMana != 0)
                {
                    if (status.Applier.GetStat(StatType.MP) < status.UpkeepMana)
                    {
                        status.Applier.Trigger(TriggerType.OnNoUpkeepMana, status: status);
                        continue;
                    }
                    status.Applier.Stats.ChangeStat(StatType.MP, -status.UpkeepMana);
                }
            }

            if (status.DecayTrigger == timing)
            {
                if (status.DecayTrigger == TimingType.OnApplierTurnBegin && status.Applier != applier)
                {
                    continue;
                }

                else if (status.Stack > 0)
                {
                    float decayAmount = status.DecayStatType == StatType.NON ?
                        1 : 
                        Owner.GetStat(status.DecayStatType) / 100f;

                    ModifyStatus(status, -decayAmount);
                }
            }
        }
    }

    public void ApplyStatus(Status status, bool isApply=true)
    {
        if (Owner.IsDead) return;
        var existing = Statuses.Find(s => s.Id == status.Id);

        if (existing == null)
        {
            if (status.Stack > 0)
            {
                Statuses.Add(status);
                ModifyStatus(status, 0);
            }
        }
        else if (status.IsStackable)
        {
            ModifyStatus(existing, status.Stack);
        }
        else
        {
            ModifyStatus(existing, Mathf.Max(existing.Stack, status.Stack));
        }

        if (isApply) status.OnApplyEffect?.Invoke();
    }

    public void ModifyStatus(Status status, float amount)
    {
        status.Stack += amount;
        Owner.Trigger(TriggerType.OnAfterStatusStackChanged, status: status);

        if (status.Stack <= 0)
        {
            Statuses.Remove(status);
            Owner.Trigger(TriggerType.OnAfterStatusRemoved, status: status);
        }
        OnStatusChanged?.Invoke();
    }

    public void RemoveSameClassStatus(Status status)
    {
        var existing = Statuses.Find(s => s.Id == status.Id);
        if (existing != null)
        {
            RemoveStatus(existing);
        }
    }

    public void RemoveStatus(Status status)
    {
        Statuses.Remove(status);
        OnStatusChanged?.Invoke();
    }

    public Status GetStatusById(int id)
    {
        return Statuses.Find(s => s.Id == id);
    }

    public Status GetStatusByName(string name)
    {
        return Statuses.Find(s => s.Name == name);
    }
}
