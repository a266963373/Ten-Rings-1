using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SettingsManager
{
    private static string Path => System.IO.Path.Combine(Application.persistentDataPath, "settings.json");

    public static void Save(GameSettings data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path, json);
    }

    public static GameSettings Load()
    {
        if (!File.Exists(Path)) return new GameSettings(); // д╛хож╣
        string json = File.ReadAllText(Path);
        return JsonUtility.FromJson<GameSettings>(json);
    }
}

[System.Serializable]
public class GameSettings
{
    public string language = "en";
}
