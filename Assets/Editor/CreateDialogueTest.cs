using UnityEditor;
using UnityEngine;

public class CreateDialogueTest
{
    [MenuItem("Tools/Create Dialogue Test")]
    public static void CreateTest()
    {
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas が見つかりません");
            return;
        }

        // 既存のスクリプトを削除
        DialogueTest existingTest = canvas.GetComponent<DialogueTest>();
        if (existingTest != null)
        {
            Object.DestroyImmediate(existingTest);
            Debug.Log("既存の DialogueTest を削除しました");
        }

        // DialogueTest をアタッチ
        canvas.gameObject.AddComponent<DialogueTest>();

        Debug.Log("✅ DialogueTest を Canvas にアタッチしました");
        Debug.Log("ゲーム中に G キーを押すとテストセリフが表示されます");
    }
}
