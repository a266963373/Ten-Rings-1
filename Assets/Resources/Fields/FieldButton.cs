using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class FieldButton : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent lse;

    private Field field;

    // 显示格式参考 FieldSystem.UpdateOneUI：UI Text 表的 "Field" 项，参数为 本地化名称 + 回合数
    public void Setup(Field f)
    {
        field = f;
        if (field == null || lse == null) return;

        string localizedName = new LocalizedString("Field", field.Name).GetLocalizedString();

        // UI 文本使用 "UI Text" 表的 "Field" 条目，参数：{0}=名称, {1}=持续回合
        lse.StringReference.Arguments = new object[] { localizedName, field.Duration };
        lse.StringReference.SetReference("UI Text", "Field");
        lse.RefreshString();
    }

    public void OnClick()
    {
        if (field == null) return;
        BattleSystem.I.FieldDescriptionPanel.ShowField(field);
    }
}
