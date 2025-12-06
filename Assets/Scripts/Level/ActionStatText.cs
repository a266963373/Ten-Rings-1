using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class ActionStatText : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    private BattleActionSO battleAction;
    private int manaCost;
    private string statString;

    public BattleActionSO BattleAction
    {
        get { return battleAction; }
        set
        {
            if (value is ActivateWeaponActionSO activateWeaponActionSO)
            {
                BattleAction = activateWeaponActionSO.GrantedActions[0];
            }
            else
            {
                battleAction = value;
                manaCost = battleAction.ManaCost;
                // 假设你有一个本地化表 "StatType"，key 为枚举名
                statString = new LocalizedString("Stat Type", battleAction.Scale.ToString()).GetLocalizedString();

                // 设置本地化参数
                localizeStringEvent.StringReference.Remove("ManaCost");
                localizeStringEvent.StringReference.Remove("StatString");
                localizeStringEvent.StringReference.Add("ManaCost", new IntVariable { Value = manaCost });
                localizeStringEvent.StringReference.Add("StatString", new StringVariable { Value = statString });

                Show();
            }
        }
    }

    private void Show()
    {
        // 假设你的本地化表 "Action Stat" 里有对应 key
        localizeStringEvent.StringReference.SetReference("Misc", "Action Stat");
        localizeStringEvent.RefreshString();
    }
}
