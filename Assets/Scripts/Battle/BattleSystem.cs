using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] Image backgroundImage;

    public BattleState State;
    public int LogCount = 0;

    [SerializeField] TimeSystem timeSystem;
    [SerializeField] ActionDecider actionDecider;
    [SerializeField] ActionResolver actionResolver;
    //[SerializeField] BattleLogSystem.I battleLogSystem;
    [SerializeField] GameObject characterInfoPanel;
    public StatusDescriptionPanel StatusDescriptionPanel;
    public FieldDescriptionPanel FieldDescriptionPanel;

    public bool IsStarted = false;
    public List<Character> Characters => battleLoader.Characters;
    private bool hasCheckedEnd = false;

    private void Awake()
    {
        if (!I) I = this;
        else Destroy(gameObject);
        characterInfoPanel.SetActive(false);
        StatusDescriptionPanel.gameObject.SetActive(false);
        if (BattleSession.BackgroundImage != null)
        {
            backgroundImage.sprite = BattleSession.BackgroundImage;
            // if not, just keep the default background
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => battleLoader.IsStarted);

        timeSystem.Initialize();
        timeSystem.OnGaugeFull += GaugeFull;

        IsStarted = true;
    }

    private void Update()
    {
        if (!IsStarted) return;
        //if (State == BattleState.Idle)
        if (LogCount == 0)
        {
            State = BattleState.Idle;
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

    private void GaugeFull(Character character)
    {
        StartCoroutine(ScGaugeFull(character));
    }

    private IEnumerator ScGaugeFull(Character character)
    {
        yield return BattleLogSystem.I.ShowWhoseTurn(character.Name, isBlock: !character.IsPlayerControlled);
        character.Trigger(timing: TimingType.OnSelfTurnBegin);
        actionDecider.Decide(character);
    }

    public void CheckBattleEnd()
    {
        bool allPlayersDead = BattleLoader.I.Allies.Count == 0;

        bool allEnemiesDead = BattleLoader.I.Enemies.Count == 0;

        if (allPlayersDead || allEnemiesDead)
        {
            BattleResult result = allEnemiesDead ? BattleResult.Win : BattleResult.Lose;

            BattleSession.Result = result;
            CalculateEncounterGold();

            SceneManager.LoadScene("LevelScene");
        }
    }

    private void CalculateEncounterGold()
    {
        int baseGold = BattleSession.Encounter.Gold;
        float processingGold = baseGold;

        foreach (Character character in Characters.Concat(battleLoader.Deads).ToList())
        {
            foreach (Ring ring in character.Rings)
            {
                if (ring != null && ring.Id == 17)  // raise the stake ring
                {
                    processingGold += baseGold * ring.Power * 0.01f;
                }
            }
        }
        BattleSession.EncounterGold = Mathf.RoundToInt(processingGold);
    }
}
