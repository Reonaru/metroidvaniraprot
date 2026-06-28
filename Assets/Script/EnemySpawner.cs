using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("スポーン設定")]
    public GameObject enemyPrefab;
    public int maxEnemyCount = 3;
    public float spawnInterval = 3f;
    public Vector3 spawnOffset = Vector3.zero;

    [Header("スポーン範囲")]
    public float spawnRangeX = 20f;

    [Header("合計スポーン設定")]
    public int maxTotalSpawn = 20;
    public string onClearFlag = "";

    private float lastSpawnTime = 0f;
    private int currentEnemyCount = 0;
    private int totalSpawnedCount = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isCleared = false;

    void Start()
    {
        EnemyBase.OnAnyEnemyDeath += OnEnemyDied;
        enabled = false;
    }

    public void StartSpawning()
    {
        enabled = true;
        lastSpawnTime = Time.time;
        Debug.Log("EnemySpawner を開始しました");
    }

    void OnDestroy()
    {
        EnemyBase.OnAnyEnemyDeath -= OnEnemyDied;
    }

    void Update()
    {
        // スポーン間隔をチェック
        if (Time.time - lastSpawnTime >= spawnInterval && currentEnemyCount < maxEnemyCount && totalSpawnedCount < maxTotalSpawn)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPrefab が設定されていません");
            return;
        }

        // ランダムなX座標でスポーン
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = transform.position + new Vector3(randomX, spawnOffset.y, spawnOffset.z);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        newEnemy.name = enemyPrefab.name + "_" + (totalSpawnedCount + 1);
        newEnemy.transform.SetParent(transform);

        spawnedEnemies.Add(newEnemy);
        lastSpawnTime = Time.time;
        currentEnemyCount++;
        totalSpawnedCount++;

        Debug.Log($"敵をスポーン: {newEnemy.name} (現在: {currentEnemyCount}/{maxEnemyCount}, 合計: {totalSpawnedCount}/{maxTotalSpawn})");
    }

    void OnEnemyDied(GameObject enemy)
    {
        if (spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Remove(enemy);
            currentEnemyCount--;
            Debug.Log($"敵が死んだ: {enemy.name} (残り: {currentEnemyCount}, 合計スポーン: {totalSpawnedCount}/{maxTotalSpawn})");

            // 合計スポーン数に達し、全敵が消滅したかチェック
            if (totalSpawnedCount >= maxTotalSpawn && spawnedEnemies.Count == 0 && !isCleared)
            {
                isCleared = true;
                if (!string.IsNullOrEmpty(onClearFlag))
                {
                    Gmanager.Instance.SetFlag(onClearFlag, true);
                    Debug.Log($"フラグ '{onClearFlag}' を立てました");
                }
                enabled = false;
            }
        }
    }

    public void SetMaxEnemyCount(int count)
    {
        maxEnemyCount = count;
    }

    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }
}
