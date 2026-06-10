using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CreateDebugCameraUI
{
    [MenuItem("Tools/Create Debug Camera UI")]
    public static void CreateDebugUI()
    {
        Debug.Log("🔵 CreateDebugUI スクリプトが実行されました");

        // 既存のDebugCanvasがあれば削除
        GameObject existingDebugCanvas = GameObject.Find("DebugCanvas");
        if (existingDebugCanvas != null)
        {
            Object.DestroyImmediate(existingDebugCanvas);
            Debug.Log("既存のDebugCanvasを削除しました");
        }

        // 既存のDebugTextがあれば削除
        GameObject existingText = GameObject.Find("DebugText");
        if (existingText != null)
        {
            Object.DestroyImmediate(existingText);
            Debug.Log("既存のDebugTextを削除しました");
        }

        // 既存のCanvasを探す
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ シーン内にCanvasが見つかりません。Canvasを作成してください。");
            return;
        }
        Debug.Log("✅ Canvas を発見しました");

        // Text オブジェクトを作成（既存のCanvasの子オブジェクト）
        GameObject textObj = new GameObject("DebugText");
        if (textObj == null)
        {
            Debug.LogError("❌ DebugText オブジェクトの作成に失敗しました");
            return;
        }
        textObj.transform.SetParent(canvas.transform, false);
        Debug.Log("✅ DebugText オブジェクトを作成しました");

        Text text = textObj.AddComponent<Text>();
        text.text = "Debug Info";
        // フォントはnullのままでUnityのデフォルトフォントを使う
        text.fontSize = 12;
        text.fontStyle = FontStyle.Normal;
        text.alignment = TextAnchor.UpperLeft;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        // RectTransform を設定（左上にアンカー）
        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(10, -10);
        rectTransform.sizeDelta = new Vector2(300, 150);

        // DebugCameraUI スクリプトをアタッチ
        DebugCameraUI debugUI = canvas.gameObject.AddComponent<DebugCameraUI>();
        if (debugUI == null)
        {
            Debug.LogError("❌ DebugCameraUI スクリプトのアタッチに失敗しました");
            return;
        }
        Debug.Log("✅ DebugCameraUI を Canvas にアタッチしました");

        debugUI.debugText = text;
        if (debugUI.debugText == null)
        {
            Debug.LogError("❌ debugText の設定に失敗しました");
            return;
        }
        Debug.Log("✅ debugText を設定しました");

        // CameraManager1 と RoomManager を自動検索して設定
        CameraManager1 cameraManager = Object.FindObjectOfType<CameraManager1>();
        RoomManager roomManager = Object.FindObjectOfType<RoomManager>();

        if (cameraManager != null)
        {
            debugUI.cameraManager = cameraManager;
        }
        else
        {
            Debug.LogWarning("CameraManager1が見つかりませんでした");
        }

        if (roomManager != null)
        {
            debugUI.roomManager = roomManager;
        }
        else
        {
            Debug.LogWarning("RoomManagerが見つかりませんでした");
        }

        // シーンをダーティにしてセーブを促す
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("✅ Debug Camera UI を作成しました");
    }
}
