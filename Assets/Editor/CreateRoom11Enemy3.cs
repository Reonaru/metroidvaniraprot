using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom11Enemy3
{
    [MenuItem("Tools/Create Room11 Enemy3")]
    public static void CreateEnemy3()
    {
        // 既存の敵を削除
        GameObject existingEnemy = GameObject.Find("Room11_Enemy3");
        if (existingEnemy != null)
        {
            Object.DestroyImmediate(existingEnemy);
            Debug.Log("既存の Room11_Enemy3 を削除しました");
        }

        GameObject obj = new GameObject("Room11_Enemy3");
        obj.AddComponent<Rigidbody2D>();
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<Enemy3>();

        // Rigidbody2D の設定
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // BoxCollider2D の設定
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(0.64f, 0.64f);

        // SpriteRenderer の設定
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        // Enemy.prefab からスプライトを取得してコピー
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
        enemy.hp = 15;
        enemy.moveSpeed = 4f;
        enemy.damageAmount = 3f;
        enemy.moveRange = 10f;

        // Room11 右寄りに配置（X = 390, Y = 0）
        obj.transform.position = new Vector3(390, 0, 0);
        obj.tag = "enemy";

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room11 に Enemy3 を作成しました：位置 (390, 0, 0)");
    }
}
