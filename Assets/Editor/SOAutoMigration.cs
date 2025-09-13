using UnityEditor;
using UnityEngine;
using System.IO;

public class SOAutoMigration
{
    [MenuItem("Tools/Migrate CharacterSO to HumanSO")]
    public static void MigrateCharacterSO()
    {
        string[] guids = AssetDatabase.FindAssets("t:CharacterSO");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CharacterSO oldSO = AssetDatabase.LoadAssetAtPath<CharacterSO>(path);

            // 新建 HumanSO
            HumanSO newSO = ScriptableObject.CreateInstance<HumanSO>();
            // 复制字段
            newSO.CharacterName = oldSO.CharacterName;
            newSO.StatEntries = new System.Collections.Generic.List<StatEntry>(oldSO.StatEntries);
            newSO.RingIds = (int[])oldSO.RingIds.Clone();

            // 保存新 SO
            string newPath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + "_Human.asset";
            AssetDatabase.CreateAsset(newSO, newPath);
            AssetDatabase.SaveAssets();
        }
        Debug.Log("批量迁移完成！");
    }
}