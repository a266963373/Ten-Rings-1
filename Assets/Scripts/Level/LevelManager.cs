using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager I { get; private set; }
    [SerializeField] EncounterPanel encounterPanel;
    [SerializeField] RewardPanel rewardPanel;
    [SerializeField] RingPanel ringPanel;
    [SerializeField] PenaltyPanel penaltyPanel;
    [SerializeField] WithdrawPanel withdrawPanel;
    private LevelSO level;
    private RunSession run;

    private void Awake()
    {
        if (!I) I = this;
        else Destroy(gameObject);

        encounterPanel.gameObject.SetActive(false);
        rewardPanel.gameObject.SetActive(false);
        ringPanel.gameObject.SetActive(false);
        penaltyPanel.gameObject.SetActive(false);
        withdrawPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        run = GameSystem.I.Run;
        level = run.Level;

        rewardPanel.OnClaimReward += RewardClaimed;
        BattleSession.IsLastEncounter = run.EncounterIndex == level.Encounters.Count - 1;

        if (run.EncounterIndex == 0)
        {
            GameSystem.I.Run.WornRingIds = GameSystem.I.CurrentSave.WornRingIds.ToArray();
            GameSystem.I.Run.StoredRingIds.Clear();
        }

        switch (BattleSession.Result)
        {
            case BattleResult.Win:
                ShowReward();
                break;

            case BattleResult.Lose:
                ShowPenalty();
                break;

            case BattleResult.None:
                ShowEncounter();
                break;
        }

        // Reset outcome for next time
        BattleSession.Result = BattleResult.None;
    }

    private void ShowEncounter()
    {
        encounterPanel.Encounter = level.Encounters[run.EncounterIndex];
        encounterPanel.gameObject.SetActive(true);
    }

    private void ShowReward()
    {
        rewardPanel.gameObject.SetActive(true);
    }

    private void RewardClaimed()
    {
        rewardPanel.gameObject.SetActive(false);
        run.EncounterIndex++;
        BattleSession.IsLastEncounter = run.EncounterIndex == level.Encounters.Count - 1;

        if (rewardPanel.IsRingSelected)
        {
            ShowRingPanel();
        }
        else
        {
            ShowEncounter();
        }

    }

    public void ShowRingPanel()
    {
        ringPanel.gameObject.SetActive(true);
    }

    public void ExitRingPanel()
    {
        ringPanel.gameObject.SetActive(false);
        ShowEncounter();
    }

    private void ShowPenalty()
    {
        penaltyPanel.gameObject.SetActive(true);
        penaltyPanel.Encounter = level.Encounters[run.EncounterIndex];
    }
}
