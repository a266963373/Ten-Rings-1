using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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
    [SerializeField] BattleLogSystem battleLogSystem;

    private List<Character> characters = new();
    private bool hasCheckedEnd = false;

    private void Awake()
    {
        if (!I) I = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadBattle();

        timeSystem.Initialize(characters);
        timeSystem.OnGaugeFull += GaugeFull;

        actionDecider.Initialize(characters);
    }

    private void Update()
    {
        if (State == BattleState.Idle)
        {
            if (!hasCheckedEnd)
            {
                hasCheckedEnd = true;
                CheckBattleEnd();
            }

            timeSystem.Tick();
        }
        else
        {
            hasCheckedEnd = false; // 状态离开 Idle，允许下次再检查
        }
    }

    private void LoadBattle()
    {
        Character player = battleLoader.Player;
        Character enemy = battleLoader.Enemy;
        
        characters.Add(player);
        characters.Add(enemy);
    }

    private void GaugeFull(Character character)
    {
        battleLogSystem.ShowWhoseTurn(character.Name, () => {
            actionDecider.Decide(character);
        }, isBlock: !character.IsPlayerControlled
        );
    }

    private void CheckBattleEnd()
    {
        bool allPlayersDead = characters
            .Where(c => c.IsPlayerSide)
            .All(c => c.IsDead);

        bool allEnemiesDead = characters
            .Where(c => !c.IsPlayerSide)
            .All(c => c.IsDead);

        if (allPlayersDead || allEnemiesDead)
        {
            BattleResult result = allEnemiesDead ? BattleResult.Win : BattleResult.Lose;

            BattleSession.Result = result;

            SceneManager.LoadScene("LevelScene");
        }
    }
}
