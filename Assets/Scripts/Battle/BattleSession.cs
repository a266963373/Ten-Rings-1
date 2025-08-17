using UnityEngine;

public enum BattleResult { None, Win, Lose }

public static class BattleSession
{
    public static EncounterSO Encounter;
    public static BattleResult Result;
    public static bool IsLastEncounter = false;
    public static bool IsDebug = false;

    static BattleSession()
    {
        Encounter = Resources.Load<EncounterSO>("ScriptableObjects/Levels/EncounterDebug");
        IsDebug = true;
        //Encounter = Resources.Load<EncounterSO>("ScriptableObjects/Levels/Encounter01.03");
        Result = BattleResult.None;
    }
}
