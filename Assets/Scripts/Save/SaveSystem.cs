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
            var data = JsonUtility.FromJson<SaveData>(json);

            // 检查并补齐 WornRingIds 长度
            if (data.WornRingIds == null)
            {
                data.WornRingIds = new int[10];
            }
            else if (data.WornRingIds.Length < 10)
            {
                int[] newArr = new int[10];
                for (int i = 0; i < data.WornRingIds.Length; i++)
                    newArr[i] = data.WornRingIds[i];
                // 剩余部分自动为0
                data.WornRingIds = newArr;
            }

            return data;
        }
        return new(saveId);
    }
}

[System.Serializable]
public class SaveData
{
    public int SaveId = 0;
    public int Gold = 0;
    public int[] WornRingIds = new int[10];
    public List<int> StoredRingIds = new();
    public ShopSetting ShopSetting = new();
    public List<int> UnlockedLevelIds = new() { 1 };
    public int UnlockedLevelNumber => UnlockedLevelIds.Count;

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

public class RunSession
{
    public LevelSO Level;
    public int EncounterIndex = 0;
    public int[] WornRingIds = new int[10];
    public List<int> StoredRingIds = new();
}
