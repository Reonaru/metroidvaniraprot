using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom8Dialogue
{
    [MenuItem("Tools/Create Room8 Dialogue")]
    public static void CreateDialogue()
    {
        // 既存のダイアログを削除
        GameObject existingDialogue = GameObject.Find("Room8_DialogueController");
        if (existingDialogue != null)
        {
            Object.DestroyImmediate(existingDialogue);
            Debug.Log("既存の Room8_DialogueController を削除しました");
        }

        // ダイアログコントローラーを作成
        GameObject dialogueObj = new GameObject("Room8_DialogueController");
        dialogueObj.transform.position = Vector3.zero;

        // Room8Dialogueスクリプトを追加
        dialogueObj.AddComponent<Room8Dialogue>();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room8 ダイアログコントローラーを作成しました");
    }
}
