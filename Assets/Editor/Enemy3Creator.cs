using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Enemy3Creator
{
    [MenuItem("Tools/Create Enemy3")]
    public static void CreateEnemy3()
    {
        // 既存のEnemy3を削除
        GameObject existingEnemy = GameObject.Find("Enemy3");
        if (existingEnemy != null)
        {
            Object.DestroyImmediate(existingEnemy);
            Debug.Log("既存のEnemy3を削除しました");
        }

        GameObject obj = new GameObject("Enemy3");
        obj.AddComponent<Rigidbody2D>();
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<Enemy3>();

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
        Enemy3 enemy = obj.GetComponent<Enemy3>();
        enemy.hp = 15;              // 弾5発で倒せる！
        enemy.moveSpeed = 4f;       // 速い！
        enemy.damage = 1;
        enemy.moveRange = 10f;      // 広い検知範囲！

        // Room3の中央付近に配置
        obj.transform.position = new Vector3(134, 1, 0);
        obj.tag = "enemy";

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Enemy3オブジェクトを作成しました：位置 (134, 1, 0) - Room3の中央、速度4、検知範囲10");
    }
}
