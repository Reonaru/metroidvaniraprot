using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CleanDebugUI
{
    [MenuItem("Tools/Clean Debug UI")]
    public static void CleanDebugUI_Menu()
    {
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas が見つかりません");
            return;
        }

        // すべての DebugText を削除
        Text[] allTexts = canvas.GetComponentsInChildren<Text>();
        int count = 0;
        foreach (Text t in allTexts)
        {
            if (t.gameObject.name == "DebugText")
            {
                Object.DestroyImmediate(t.gameObject);
                count++;
            }
        }
        Debug.Log($"DebugText を {count} 個削除しました");

        // すべての DebugCameraUI を削除
        DebugCameraUI[] debugUIs = Object.FindObjectsOfType<DebugCameraUI>();
        foreach (DebugCameraUI ui in debugUIs)
        {
            Object.DestroyImmediate(ui);
        }
        Debug.Log($"DebugCameraUI を {debugUIs.Length} 個削除しました");
    }
}
