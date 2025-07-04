using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem I { get; private set; }

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

    public SaveData CurrentSave { get; private set; }

    public void LoadSaveData(int saveId)
    {
        CurrentSave = SaveSystem.I.Load(saveId);
    }
}
