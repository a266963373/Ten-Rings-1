using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI worldTurnTmp;
    [SerializeField] BarController worldTimeBar;

    private List<Character> characters => BattleSystem.I.Characters;
    public event Action<Character> OnGaugeFull;

    private float actionThreshold = 100f;
    private int currentIndex = 0; // 从这个角色开始
    private float worldTime = 0;
    private int worldTurn = 1;

    public void Initialize()
    {
        currentIndex = 0;
        worldTime = 0;
    }

    public void Tick()
    {
        if (characters == null || characters.Count == 0) return;

        while (currentIndex < characters.Count)
        {
            var c = characters[currentIndex];

            if (!c.IsDead)
            {
                c.ActionGauge += c.Stats.GetStat(StatType.SPD) * Time.deltaTime;

                if (c.ActionGauge >= actionThreshold)
                {
                    c.ActionGauge = 0;
                    BattleSystem.I.LogCount++;
                    BattleSystem.I.State = BattleState.AwaitForAction;
                    OnGaugeFull?.Invoke(c);

                    // 下次从下一个角色开始
                    currentIndex++;
                    return; // 立刻退出 Tick，等待下一帧
                }
            }

            // 检查下一个
            currentIndex++;
        }
        currentIndex = 0;
        worldTime += 100 * Time.deltaTime;
        if (worldTime >= 100)
        {
            worldTime = 0;
            worldTurn++;
            OnWorldTurn();
        }
        UpdateWorldTimeUI();
    }

    private void UpdateWorldTimeUI()
    {
        worldTurnTmp.text = worldTurn.ToString();
        worldTimeBar.LeftNum = worldTime;
        worldTimeBar.LazyUpdate();
    }

    private void OnWorldTurn()
    {
        foreach (Character c in characters)
        {
            c.OnWorldTurn();
        }
    }
}
