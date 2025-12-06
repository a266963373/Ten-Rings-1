using UnityEngine;
using UnityEditor;

public class ConvertSkillRingToWeaponRingSO
{
    [MenuItem("Assets/将选中的 SkillRingSO 转为 WeaponRingSO", true)]
    private static bool ValidateConvert()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is ScriptableObject so && so.GetType().Name == "SkillRingSO")
                return true;
        }
        return false;
    }

    [MenuItem("Assets/将选中的 SkillRingSO 转为 WeaponRingSO")]
    private static void ConvertSelectedSkillRings()
    {
        var weaponScript = AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Resources/RingRelated/WeaponRingSO.cs");
        if (weaponScript == null)
        {
            Debug.LogError("找不到 WeaponRingSO 脚本！");
            return;
        }

        foreach (var obj in Selection.objects)
        {
            if (obj is ScriptableObject so && so.GetType().Name == "SkillRingSO")
            {
                var soPath = AssetDatabase.GetAssetPath(so);
                var serializedObject = new SerializedObject(so);
                serializedObject.FindProperty("m_Script").objectReferenceValue = weaponScript;
                serializedObject.ApplyModifiedProperties();
                Debug.Log($"已将 {soPath} 类型迁移为 WeaponRingSO");
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
