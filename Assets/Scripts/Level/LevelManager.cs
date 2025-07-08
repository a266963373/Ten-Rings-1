using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager I { get; private set; }
    [SerializeField] EncounterPanel encounterPanel;
    public LevelSO Level;
    private int currentEncounterIndex = 0;

    private void Awake()
    {
        if (!I) I = this;
        else Destroy(gameObject);

        encounterPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        switch (BattleSession.Result)
        {
            case BattleResult.Win:
                //ShowReward(BattleSession.Encounter, () => ProceedToNextEnemy());
                break;

            case BattleResult.Lose:
                //ShowPenalty(BattleSession.Encounter, () => RetryOrExit());
                break;

            case BattleResult.None:
                //DisplayNextEncounter();
                break;
        }

        // Reset outcome for next time
        BattleSession.Result = BattleResult.None;
    }

    private void DisplayNextEncounter()
    {
        encounterPanel.Encounter = Level.Encounters[currentEncounterIndex];
        encounterPanel.gameObject.SetActive(true);
    }
}
