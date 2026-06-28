using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom3DefeatedDialogue
{
    [MenuItem("Tools/Create Room3 Defeated Dialogue")]
    public static void CreateDialogue()
    {
        // 既存のダイアログを削除
        GameObject existingDialogue = GameObject.Find("Room3_DefeatedDialogueController");
        if (existingDialogue != null)
        {
            Object.DestroyImmediate(existingDialogue);
            Debug.Log("既存の Room3_DefeatedDialogueController を削除しました");
        }

        // ダイアログコントローラーを作成
        GameObject dialogueObj = new GameObject("Room3_DefeatedDialogueController");
        dialogueObj.transform.position = Vector3.zero;

        // Enemy3DefeatedDialogueスクリプトを追加
        dialogueObj.AddComponent<Enemy3DefeatedDialogue>();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room3 敵撃破ダイアログコントローラーを作成しました");
    }
}
