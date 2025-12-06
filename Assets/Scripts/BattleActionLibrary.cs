using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionLibrary : MonoBehaviour
{
    public static BattleActionLibrary I { get; private set; }

    private Dictionary<string, BattleActionSO> actionTemplates = new();
    public int ActionCount => actionTemplates.Count;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ActivateWeaponActionSO ActivateWeaponActionSO;
}
