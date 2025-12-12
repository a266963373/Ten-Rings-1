using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SOAutoMigration
{
    [MenuItem("Assets/Convert to CharacterSO", true)]
    private static bool ValidateConvertToCharacterSO()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is ScriptableObject)
                return true;
        }
        return false;
    }

    [MenuItem("Assets/Convert to CharacterSO")]
    public static void ConvertToCharacterSO()
    {
        int convertedCount = 0;
        List<Object> newAssets = new List<Object>();

        foreach (var obj in Selection.objects)
        {
            ScriptableObject selected = obj as ScriptableObject;
            if (selected == null)
                continue;

            string path = AssetDatabase.GetAssetPath(selected);
            string oldName = Path.GetFileNameWithoutExtension(path);

            // 创建新的 CharacterSO
            CharacterSO newSO = ScriptableObject.CreateInstance<CharacterSO>();

            // 尝试从原 SO 复制字段
            CopyCharacterFields(selected, newSO);

            // 保存新 SO,使用 {OldName}SO 命名规则
            string newPath = Path.GetDirectoryName(path) + "/" + oldName + "CharacterSO.asset";
            AssetDatabase.CreateAsset(newSO, newPath);
            newAssets.Add(newSO);
            convertedCount++;

            Debug.Log($"已转换为 CharacterSO: {newPath}");
        }

        AssetDatabase.SaveAssets();

        // 选中所有新创建的资产
        if (newAssets.Count > 0)
        {
            Selection.objects = newAssets.ToArray();
        }

        Debug.Log($"批量转换完成! 共转换 {convertedCount} 个资产为 CharacterSO");
    }

    [MenuItem("Assets/Convert to CharacterTemplateSO", true)]
    private static bool ValidateConvertToCharacterTemplateSO()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is ScriptableObject)
                return true;
        }
        return false;
    }

    [MenuItem("Assets/Convert to CharacterTemplateSO")]
    public static void ConvertToCharacterTemplateSO()
    {
        int convertedCount = 0;
        List<Object> newAssets = new List<Object>();

        foreach (var obj in Selection.objects)
        {
            ScriptableObject selected = obj as ScriptableObject;
            if (selected == null)
                continue;

            string path = AssetDatabase.GetAssetPath(selected);
            string oldName = Path.GetFileNameWithoutExtension(path);

            // 创建新的 CharacterTemplateSO
            CharacterTemplateSO newSO = ScriptableObject.CreateInstance<CharacterTemplateSO>();

            // 尝试从原 SO 复制字段
            CopyCharacterFields(selected, newSO);

            // 保存新 SO,使用 {OldName}TemplateSO 命名规则
            string newPath = Path.GetDirectoryName(path) + "/" + oldName + "CharacterTemplateSO.asset";
            AssetDatabase.CreateAsset(newSO, newPath);
            newAssets.Add(newSO);
            convertedCount++;

            Debug.Log($"已转换为 CharacterTemplateSO: {newPath}");
        }

        AssetDatabase.SaveAssets();

        // 选中所有新创建的资产
        if (newAssets.Count > 0)
        {
            Selection.objects = newAssets.ToArray();
        }

        Debug.Log($"批量转换完成! 共转换 {convertedCount} 个资产为 CharacterTemplateSO");
    }

    [MenuItem("Assets/Set Template Field", true)]
    private static bool ValidateSetTemplateField()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is CharacterSO)
                return true;
        }
        return false;
    }

    [MenuItem("Assets/Set Template Field")]
    public static void SetTemplateField()
    {
        // 弹出输入对话框
        string templateValue = EditorInputDialog.Show("设置 Template 字段", "请输入 Template 值:", "");

        if (templateValue == null)
        {
            Debug.Log("操作已取消");
            return;
        }

        int updatedCount = 0;

        foreach (var obj in Selection.objects)
        {
            CharacterSO characterSO = obj as CharacterSO;
            if (characterSO == null)
                continue;

            characterSO.Template = templateValue;
            EditorUtility.SetDirty(characterSO);
            updatedCount++;

            Debug.Log($"已设置 {characterSO.name} 的 Template 为: {templateValue}");
        }

        AssetDatabase.SaveAssets();

        Debug.Log($"批量设置完成! 共更新 {updatedCount} 个 CharacterSO 的 Template 字段");
    }

    private static void CopyCharacterFields(ScriptableObject source, CharacterSO target)
    {
        // 使用反射复制字段
        var sourceType = source.GetType();

        // 复制 CharacterName
        var nameField = sourceType.GetField("CharacterName");
        if (nameField != null)
        {
            target.CharacterName = (string)nameField.GetValue(source);
        }

        // 复制 Template
        var templateField = sourceType.GetField("Template");
        if (templateField != null)
        {
            target.Template = (string)templateField.GetValue(source);
        }

        // 复制 StatEntries
        var statEntriesField = sourceType.GetField("StatEntries");
        if (statEntriesField != null)
        {
            var sourceStats = statEntriesField.GetValue(source) as List<StatEntry>;
            if (sourceStats != null)
            {
                target.StatEntries = new List<StatEntry>(sourceStats);
            }
        }
        else
        {
            // 如果没有 StatEntries,创建空列表
            target.StatEntries = new List<StatEntry>();
        }

        // 复制 RingIds
        var ringIdsField = sourceType.GetField("RingIds");
        if (ringIdsField != null)
        {
            var sourceRings = ringIdsField.GetValue(source) as int[];
            if (sourceRings != null)
            {
                target.RingIds = (int[])sourceRings.Clone();
            }
        }
        else
        {
            // 如果没有 RingIds,创建默认数组
            target.RingIds = new int[10];
        }
    }
}

// 简单的输入对话框工具类
public class EditorInputDialog : EditorWindow
{
    private string description = "请输入值:";
    private string inputText = "";
    private string result = null;
    private bool shouldClose = false;

    public static string Show(string title, string description, string defaultValue)
    {
        EditorInputDialog window = ScriptableObject.CreateInstance<EditorInputDialog>();
        window.titleContent = new GUIContent(title);
        window.description = description;
        window.inputText = defaultValue;
        window.result = null;
        window.shouldClose = false;

        window.ShowModal();

        return window.result;
    }

    void OnGUI()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(description, EditorStyles.wordWrappedLabel);
        EditorGUILayout.Space(10);

        GUI.SetNextControlName("InputField");
        inputText = EditorGUILayout.TextField("Template:", inputText);

        if (Event.current.type == EventType.Repaint && !shouldClose)
        {
            EditorGUI.FocusTextInControl("InputField");
        }

        // 按下 Enter 键确认
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
        {
            result = inputText;
            shouldClose = true;
            Close();
            Event.current.Use();
        }

        // 按下 Escape 键取消
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            result = null;
            shouldClose = true;
            Close();
            Event.current.Use();
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("确定", GUILayout.Width(100)))
        {
            result = inputText;
            shouldClose = true;
            Close();
        }

        if (GUILayout.Button("取消", GUILayout.Width(100)))
        {
            result = null;
            shouldClose = true;
            Close();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);
    }

    void OnDestroy()
    {
        if (!shouldClose)
        {
            result = null;
        }
    }
}