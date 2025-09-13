using System.Linq;
using UnityEngine;

public enum BattleResult { None, Win, Lose }

public static class BattleSession
{
    public static EncounterSO Encounter;
    public static int EncounterGold;    // will be set after the battle
    public static BattleResult Result;
    public static bool IsLastEncounter = false;
    public static bool IsDebug = false;
    public static Sprite BackgroundImage;

    static BattleSession()
    {
        Encounter = Resources.Load<EncounterSO>("ScriptableObjects/Levels/EncounterDebug");
        IsDebug = true;
        //Encounter = Resources.Load<EncounterSO>("ScriptableObjects/Levels/Encounter02.04");
        Result = BattleResult.None;
    }
}
