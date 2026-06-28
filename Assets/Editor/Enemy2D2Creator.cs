using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Enemy2D2Creator
{
    [MenuItem("Tools/Create Enemy2D2")]
    public static void CreateEnemy2D2()
    {
        // 既存のEnemy2D2を削除
        GameObject existingEnemy = GameObject.Find("Enemy2D2");
        if (existingEnemy != null)
        {
            Object.DestroyImmediate(existingEnemy);
            Debug.Log("既存のEnemy2D2を削除しました");
        }

        GameObject obj = new GameObject("Enemy2D2");
        obj.AddComponent<Rigidbody2D>();
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<Enemy2D2>();

        // Rigidbody2Dの設定
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // BoxCollider2Dの設定
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(0.64f, 0.64f);

        // SpriteRendererの設定
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        // Enemy.prefabからスプライトを取得してコピー
        string prefabPath = "Assets/prefab/Enemy.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab != null)
        {
            SpriteRenderer prefabSprite = prefab.GetComponent<SpriteRenderer>();
            if (prefabSprite != null && prefabSprite.sprite != null)
            {
                spriteRenderer.sprite = prefabSprite.sprite;
            }
        }

        // 敵のスクリプト設定
        Enemy2D2 enemy = obj.GetComponent<Enemy2D2>();
        enemy.hp = 3;
        enemy.moveSpeed = 2f;
        enemy.damageAmount = 3f;
        enemy.moveRange = 5f;

        // カメラ視界内に配置
        obj.transform.position = new Vector3(3, 1, 0);
        obj.tag = "enemy";

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Enemy2D2オブジェクトを作成しました：位置 (3, 1, 0)、スプライト付き");
    }
}
