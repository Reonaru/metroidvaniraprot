using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateItem
{
    [MenuItem("Tools/Create Item")]
    public static void CreateItemObject()
    {
        // 既存のアイテムを削除
        GameObject existingItem = GameObject.Find("Item");
        if (existingItem != null)
        {
            Object.DestroyImmediate(existingItem);
            Debug.Log("既存のアイテムを削除しました");
        }

        // アイテムオブジェクトを作成
        GameObject item = new GameObject("Item");
        item.transform.position = new Vector3(20, 0, 0);  // Room1 中央

        // SpriteRenderer を追加
        SpriteRenderer spriteRenderer = item.AddComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 1f, 0f);  // 黄色

        // CircleCollider2D を追加
        CircleCollider2D circleCollider = item.AddComponent<CircleCollider2D>();
        circleCollider.radius = 0.3f;
        circleCollider.isTrigger = true;

        // Rigidbody2D を追加
        Rigidbody2D rb = item.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        // Item スクリプトを追加
        Item itemScript = item.AddComponent<Item>();
        itemScript.itemValue = 1;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("アイテムを作成しました");
    }
}
