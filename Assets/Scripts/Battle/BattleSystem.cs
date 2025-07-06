using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BattleState
{
    Idle,
    AwaitForAction,
    PlayingAnimation,
    Busy,
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem I { get; private set; }
    [SerializeField] BattleLoader battleLoader;

    private TimeSystem timeSystem = new();
    public BattleState State;

    [SerializeField] ActionDecider actionDecider;
    [SerializeField] ActionResolver actionResolver;

    private List<Character> characters = new();

    private void Awake()
    {
        if (!I) I = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadBattle();

        timeSystem.Initialize(characters);
        timeSystem.OnGaugeFull += actionDecider.Decide;

        actionDecider.Initialize(characters);
    }

    private void Update()
    {
        if (State == BattleState.Idle)
        {
            timeSystem.Tick();
        }
    }

    private void LoadBattle()
    {
        Character player = battleLoader.Player;
        Character enemy = battleLoader.Enemy;
        
        characters.Add(player);
        characters.Add(enemy);
    }

    private void ResolveAction(BattleAction action)
    {

    }

    private void PerformTurn(Character actor)
    {
        // 示例：找到随机目标
        Character target = characters.Where(c => c != actor).FirstOrDefault();
        if (target != null)
        {
            Attack(actor, target);
            Debug.Log($"{actor.Name} 攻击了 {target.Name}");
        }
    }

    public void Attack(Character actor, Character target)
    {
        int dmg = actor.Stats.GetStat(StatType.STR);
        StatModifier mod = new()
        {
            IsPermanent = true,
            Value = -dmg,
        };
        target.Stats.AddModifier(mod);
    }
}
