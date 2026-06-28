using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Enemy4Creator
{
    [MenuItem("Tools/Create Enemy4")]
    public static void CreateEnemy4()
    {
        // 既存の Enemy4 を削除
        GameObject existingEnemy = GameObject.Find("Enemy4");
        if (existingEnemy != null)
        {
            Object.DestroyImmediate(existingEnemy);
            Debug.Log("既存の Enemy4 を削除しました");
        }

        // Enemy4 を作成
        GameObject enemy4 = new GameObject("Enemy4");
        enemy4.transform.position = new Vector3(280, 20, 0);  // Room8 中央、天井上

        // SpriteRenderer を追加
        SpriteRenderer spriteRenderer = enemy4.AddComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f);

        // CircleCollider2D を追加
        CircleCollider2D circleCollider = enemy4.AddComponent<CircleCollider2D>();
        circleCollider.radius = 0.5f;

        // Rigidbody2D を追加
        Rigidbody2D rb = enemy4.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;  // 重力を有効化
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Enemy4 スクリプトを追加
        Enemy4 enemy4Script = enemy4.AddComponent<Enemy4>();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Enemy4 を Room8 天井から降ってくるように配置しました");
    }
}
