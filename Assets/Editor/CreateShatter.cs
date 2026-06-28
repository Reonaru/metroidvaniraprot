using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateShatter
{
    [MenuItem("Tools/Create Shatter")]
    public static void CreateShatterObject()
    {
        // 既存のシャッターを削除（複数の場合は名前を変える）
        GameObject existingShatter = GameObject.Find("Shatter");
        if (existingShatter != null)
        {
            Object.DestroyImmediate(existingShatter);
            Debug.Log("既存のシャッターを削除しました");
        }

        // シャッターオブジェクトを作成
        GameObject shatterObj = new GameObject("Shatter");
        shatterObj.transform.position = new Vector3(0, 0, 0);

        // SpriteRenderer を追加
        SpriteRenderer spriteRenderer = shatterObj.AddComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f, 1f);  // 灰色

        // BoxCollider2D を追加（Player用・物理衝突）
        BoxCollider2D boxCollider = shatterObj.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(1f, 2f);
        boxCollider.isTrigger = false;

        // BoxCollider2D を追加（bullet用・Trigger判定）
        BoxCollider2D triggerCollider = shatterObj.AddComponent<BoxCollider2D>();
        triggerCollider.size = new Vector2(1f, 2f);
        triggerCollider.isTrigger = true;

        // Rigidbody2D を追加
        Rigidbody2D rb = shatterObj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.gravityScale = 0f;

        // DoorController スクリプトを追加
        DoorController doorController = shatterObj.AddComponent<DoorController>();

        // タグを設定
        shatterObj.tag = "shatter";

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("シャッターを作成しました");
    }
}
