using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem
{
    private List<Character> characters;
    public event Action<Character> OnGaugeFull;

    private float actionThreshold = 100f;
    private int currentIndex = 0; // 从这个角色开始

    public void Initialize(List<Character> chars)
    {
        characters = chars;
        currentIndex = 0;
    }

    public void Tick()
    {
        if (characters == null || characters.Count == 0) return;

        int checkedCount = 0;

        while (checkedCount < characters.Count)
        {
            var c = characters[currentIndex];

            c.ActionGauge += c.Stats.GetStat(StatType.SPD) * Time.deltaTime;

            if (c.ActionGauge >= actionThreshold)
            {
                c.ActionGauge = 0;
                OnGaugeFull?.Invoke(c);
                BattleSystem.I.State = BattleState.AwaitForAction;

                // 下次从下一个角色开始
                currentIndex = (currentIndex + 1) % characters.Count;
                return; // 立刻退出 Tick，等待下一帧
            }

            // 检查下一个
            currentIndex = (currentIndex + 1) % characters.Count;
            checkedCount++;
        }
    }
}
