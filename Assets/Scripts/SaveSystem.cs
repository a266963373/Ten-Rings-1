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

    public void Save(SaveData saveData, int saveId)
    {
        string path = Application.persistentDataPath + $"/save{saveId}.json";
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
    public int SaveId = 0;
    public int Gold = 0;
    public List<int> WornRingIds = new();
    public List<int> StoredRingIds = new();
    public ShopSetting ShopSetting = new();

    public SaveData(int saveId)
    {
        SaveId = saveId;
    }
}

[System.Serializable]
public class ShopSetting
{
    public int ShopLevel = 1;
    public int ShoppedNumber = 0;
    public List<int> SellingRingIds = new();
}
