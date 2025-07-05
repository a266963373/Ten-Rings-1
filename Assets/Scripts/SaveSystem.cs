using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem I { get; private set; }

    private void Awake()
    {
        if (!I)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        I = null;
    }

    public void Save(SaveData saveData)
    {
        string path = Application.persistentDataPath + $"/save{saveData.saveId}.json";
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public SaveData Load(int saveId)
    {
        string path = Application.persistentDataPath + $"/save{saveId}.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return new(saveId);
    }
}

[System.Serializable]
public class SaveData
{
    public int saveId;
    public int gold = 0;

    public SaveData(int saveId)
    {
        this.saveId = saveId;
    }
}
