using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class CreateHPBar
{
    [MenuItem("Tools/Create HP Bar")]
    public static void CreateHPBarUI()
    {
        // Canvas を探す
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            Debug.LogError("Canvas という名前のオブジェクトが見つかりません");
            return;
        }

        Canvas canvas = canvasObj.GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas コンポーネントが見つかりません");
            return;
        }

        // 既存の HPBar を削除
        Transform existingHPBar = canvas.transform.Find("HPBar");
        if (existingHPBar != null)
        {
            Object.DestroyImmediate(existingHPBar.gameObject);
            Debug.Log("既存の HPBar を削除しました");
        }

        // 背景バーのオブジェクトを作成
        GameObject backgroundObj = new GameObject("HPBarBackground");
        backgroundObj.transform.SetParent(canvas.transform, false);

        RectTransform bgRectTransform = backgroundObj.AddComponent<RectTransform>();
        bgRectTransform.anchorMin = new Vector2(0, 1);
        bgRectTransform.anchorMax = new Vector2(0, 1);
        bgRectTransform.pivot = new Vector2(0, 1);
        bgRectTransform.anchoredPosition = new Vector2(10, -12);
        bgRectTransform.sizeDelta = new Vector2(37, 15);

        Image bgImage = backgroundObj.AddComponent<Image>();
        bgImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);  // 灰色（透明）

        // HPBar オブジェクトを作成
        GameObject hpBarObj = new GameObject("HPBar");
        hpBarObj.transform.SetParent(backgroundObj.transform, false);
        hpBarObj.transform.localPosition = new Vector3(0, 0, 0);

        // RectTransform を設定（位置とサイズ）
        RectTransform rectTransform = hpBarObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);  // 左下
        rectTransform.anchorMax = new Vector2(0, 1);  // 左上
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(37, 0);

        // Image コンポーネントを追加
        Image hpBarImage = hpBarObj.AddComponent<Image>();
        hpBarImage.color = new Color(0f, 1f, 0f, 1f);  // 緑色

        // HPBar スクリプトを追加
        hpBarObj.AddComponent<HPBar>();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("HPBar を作成しました");
    }
}
