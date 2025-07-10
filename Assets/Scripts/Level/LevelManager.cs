using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager I { get; private set; }
    [SerializeField] EncounterPanel encounterPanel;
    [SerializeField] RewardPanel rewardPanel;
    [SerializeField] RingPanel ringPanel;
    public LevelSO Level;
    private int currentEncounterIndex = 0;

    private void Awake()
    {
        if (!I) I = this;
        else Destroy(gameObject);

        encounterPanel.gameObject.SetActive(false);
        rewardPanel.gameObject.SetActive(false);
        ringPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        rewardPanel.OnClaimReward += RewardClaimed;
        rewardPanel.IsLastEncounter = currentEncounterIndex == Level.Encounters.Count - 1;
        if (currentEncounterIndex == 0)
        {
            GameSystem.I.Run.WornRingIds.Clear();
            GameSystem.I.Run.StoredRingIds.Clear();
        }

        switch (BattleSession.Result)
        {
            case BattleResult.Win:
                //ShowReward(BattleSession.Encounter, () => ProceedToNextEnemy());
                ShowReward();
                break;

            case BattleResult.Lose:
                //ShowPenalty(BattleSession.Encounter, () => RetryOrExit());
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
        encounterPanel.Encounter = Level.Encounters[currentEncounterIndex];
        encounterPanel.gameObject.SetActive(true);
    }

    private void ShowReward()
    {
        rewardPanel.gameObject.SetActive(true);
    }

    private void RewardClaimed()
    {
        rewardPanel.gameObject.SetActive(false);
        currentEncounterIndex++;
        if (currentEncounterIndex >= Level.Encounters.Count)
        {

        }


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
}
