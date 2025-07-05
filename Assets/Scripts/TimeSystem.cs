using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem
{
    private List<Character> characters;
    public event Action<Character> OnGaugeFull;
    private float actionThreshold = 100f;

    public void Initialize(List<Character> chars)
    {
        characters = chars;
    }

    public void Tick()
    {
        foreach (var c in characters)
        {
            c.ActionGauge += c.Stats.GetStat(StatType.SPD) * Time.deltaTime;
            if (c.ActionGauge >= actionThreshold)
            {
                c.ActionGauge = 0;
                OnGaugeFull?.Invoke(c);
            }
        }
    }
}
