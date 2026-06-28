using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom11EnemySpawner
{
    [MenuItem("Tools/Create Room11 Enemy Spawner")]
    public static void CreateSpawner()
    {
        // 既存のスポーナーを削除
        GameObject existingSpawner = GameObject.Find("Room11_EnemySpawner");
        if (existingSpawner != null)
        {
            Object.DestroyImmediate(existingSpawner);
            Debug.Log("既存の Room11_EnemySpawner を削除しました");
        }

        // Enemy2D プレハブの作成または読み込み
        string prefabPath = "Assets/prefab/Enemy2D.prefab";
        GameObject enemy2dPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (enemy2dPrefab == null)
        {
            enemy2dPrefab = CreateEnemy2DPrefab(prefabPath);
        }

        // スポーナーオブジェクトを作成
        GameObject spawnerObj = new GameObject("Room11_EnemySpawner");
        spawnerObj.transform.position = new Vector3(390, 0, 0);

        // EnemySpawner スクリプトを追加
        EnemySpawner spawner = spawnerObj.AddComponent<EnemySpawner>();

        // スポーナー設定
        spawner.maxEnemyCount = 5;
        spawner.spawnInterval = 3f;
        spawner.spawnRangeX = 20f;
        spawner.spawnOffset = Vector3.zero;
        spawner.enemyPrefab = enemy2dPrefab;
        spawner.maxTotalSpawn = 20;
        spawner.onClearFlag = "Room11_Clear";

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room11 に EnemySpawner を作成しました（Enemy2D プレハブ自動配置済み）");
    }

    private static GameObject CreateEnemy2DPrefab(string prefabPath)
    {
        // 一時的にシーンに Enemy2D を作成
        GameObject tempEnemy = new GameObject("Enemy2D");
        tempEnemy.AddComponent<Rigidbody2D>();
        tempEnemy.AddComponent<BoxCollider2D>();
        tempEnemy.AddComponent<SpriteRenderer>();
        tempEnemy.AddComponent<Enemy2D>();

        // Rigidbody2D の設定
        Rigidbody2D rb = tempEnemy.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // BoxCollider2D の設定
        BoxCollider2D collider = tempEnemy.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(0.64f, 0.64f);

        // SpriteRenderer の設定
        SpriteRenderer spriteRenderer = tempEnemy.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        // Enemy.prefab からスプライトを取得
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/prefab/Enemy.prefab");
        if (prefab != null)
        {
            SpriteRenderer prefabSprite = prefab.GetComponent<SpriteRenderer>();
            if (prefabSprite != null && prefabSprite.sprite != null)
            {
                spriteRenderer.sprite = prefabSprite.sprite;
            }
        }

        // Enemy2D 設定
        Enemy2D enemy = tempEnemy.GetComponent<Enemy2D>();
        enemy.hp = 3;
        enemy.moveSpeed = 2f;
        enemy.damageAmount = 3f;
        enemy.moveRange = 5f;

        // プレハブとして保存
        GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(tempEnemy, prefabPath);
        Object.DestroyImmediate(tempEnemy);

        Debug.Log("Enemy2D プレハブを作成しました：" + prefabPath);
        return savedPrefab;
    }
}
