using UnityEngine;

public enum BattleResult { None, Win, Lose }

public static class BattleSession
{
    public static EncounterSO Encounter;
    public static BattleResult Result;

    static BattleSession()
    {
        Encounter = Resources.Load<EncounterSO>("ScriptableObjects/Levels/EncounterDebug");
        Result = BattleResult.None;
    }
}
