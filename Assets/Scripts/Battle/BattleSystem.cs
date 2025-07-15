using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public enum BattleState
{
    Idle,
    AwaitForAction,
    ActionResolved,
    PlayingAnimation,
    Busy,
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem I { get; private set; }
    [SerializeField] BattleLoader battleLoader;

    public BattleState State;
    public bool IsLogging = false;

    [SerializeField] TimeSystem timeSystem;
    [SerializeField] ActionDecider actionDecider;
    [SerializeField] ActionResolver actionResolver;
    [SerializeField] BattleLogSystem battleLogSystem;
    [SerializeField] GameObject characterInfoPanel;

    public bool IsStarted = false;

    private List<Character> characters = new();
    private bool hasCheckedEnd = false;

    private void Awake()
    {
        if (!I) I = this;
        else Destroy(gameObject);
        characterInfoPanel.SetActive(false);
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => battleLoader.IsStarted);
        LoadBattle();

        timeSystem.Initialize(characters);
        timeSystem.OnGaugeFull += GaugeFull;

        actionDecider.Initialize(characters);
        IsStarted = true;
    }

    private void Update()
    {
        if (!IsStarted) return;
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
            if (!IsLogging) State = BattleState.Idle;
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
        StartCoroutine(ScGaugeFull(character));
    }

    private IEnumerator ScGaugeFull(Character character)
    {
        yield return battleLogSystem.ShowWhoseTurn(character.Name, isBlock: !character.IsPlayerControlled);
        actionDecider.Decide(character);
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
