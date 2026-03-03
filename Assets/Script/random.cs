using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random : MonoBehaviour
{
    [Header("アイテム設定")]
    [SerializeField] private GameObject[] itemPrefabs;  // 配置するアイテムのプリファブ
    [SerializeField] private int maxItemCount = 10;     // 最大配置数
    [SerializeField] private int minItemCount = 5;      // 最小配置数
    
    [Header("配置範囲設定（2D）")]
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10, -5);  // 配置範囲の最小座標（X, Y）
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(10, 5);    // 配置範囲の最大座標（X, Y）
    [SerializeField] private float fixedZPosition = 0f;                    // 固定するZ座標
    
    [Header("配置条件")]
    [SerializeField] private LayerMask obstacleLayerMask = 0;    // 障害物のレイヤー
    [SerializeField] private float minDistanceBetweenItems = 2f; // アイテム間の最小距離
    [SerializeField] private float overlapCheckRadius = 1f;     // 重複チェックの半径
    
    [Header("配置方法")]
    [SerializeField] private bool spawnOnStart = true;          // 開始時に自動配置
    [SerializeField] private bool avoidObstacles = true;        // 障害物を避ける
    [SerializeField] private int maxSpawnAttempts = 50;         // 配置試行回数の上限
    
    [Header("デバッグ")]
    [SerializeField] private bool showDebugLog = true;
    [SerializeField] private bool showGizmos = true;
    
    private List<GameObject> spawnedItems = new List<GameObject>();
    
    void Start()
    {
        if (spawnOnStart)
        {
            SpawnRandomItems();
        }
    }
    
    /// <summary>
    /// ランダムにアイテムを配置
    /// </summary>
    public void SpawnRandomItems()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0)
        {
            Debug.LogWarning("アイテムプリファブが設定されていません");
            return;
        }
        
        // 既存のアイテムを削除
        ClearSpawnedItems();
        
        // 配置するアイテム数を決定
        int itemCount = Random.Range(minItemCount, maxItemCount + 1);
        
        if (showDebugLog)
        {
            Debug.Log($"アイテムを {itemCount} 個配置開始");
        }
        
        int successfulSpawns = 0;
        int attempts = 0;
        
        while (successfulSpawns < itemCount && attempts < maxSpawnAttempts)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            
            if (IsValidSpawnPosition(spawnPosition))
            {
                GameObject itemPrefab = GetRandomItemPrefab();
                GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
                
                spawnedItems.Add(spawnedItem);
                successfulSpawns++;
                
                if (showDebugLog)
                {
                    Debug.Log($"アイテム '{itemPrefab.name}' を ({spawnPosition.x}, {spawnPosition.y}) に配置");
                }
            }
            
            attempts++;
        }
        
        if (showDebugLog)
        {
            Debug.Log($"配置完了: {successfulSpawns}/{itemCount} 個 (試行回数: {attempts})");
        }
    }
    
    /// <summary>
    /// ランダムな配置座標を取得（2D）
    /// </summary>
    /// <returns>ランダムな座標</returns>
    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        
        return new Vector3(x, y, fixedZPosition);
    }
    
    /// <summary>
    /// 配置位置が有効かチェック（2D）
    /// </summary>
    /// <param name="position">チェックする座標</param>
    /// <returns>有効な場合はtrue</returns>
    private bool IsValidSpawnPosition(Vector3 position)
    {
        // 障害物チェック（2Dの円形チェック）
        if (avoidObstacles)
        {
            Collider2D obstacle = Physics2D.OverlapCircle(position, overlapCheckRadius, obstacleLayerMask);
            if (obstacle != null)
            {
                return false;
            }
        }
        
        // 他のアイテムとの距離チェック
        foreach (GameObject item in spawnedItems)
        {
            if (item != null)
            {
                float distance = Vector2.Distance(position, item.transform.position);
                if (distance < minDistanceBetweenItems)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// ランダムなアイテムプリファブを取得
    /// </summary>
    /// <returns>ランダムに選ばれたプリファブ</returns>
    private GameObject GetRandomItemPrefab()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Length);
        return itemPrefabs[randomIndex];
    }
    
    /// <summary>
    /// 配置済みアイテムを全て削除
    /// </summary>
    public void ClearSpawnedItems()
    {
        foreach (GameObject item in spawnedItems)
        {
            if (item != null)
            {
                DestroyImmediate(item);
            }
        }
        
        spawnedItems.Clear();
        
        if (showDebugLog)
        {
            Debug.Log("配置済みアイテムを全て削除しました");
        }
    }
    
    /// <summary>
    /// 特定の座標にアイテムを配置（2D）
    /// </summary>
    /// <param name="position">配置座標（X, Y）</param>
    /// <returns>配置されたアイテム</returns>
    public GameObject SpawnItemAtPosition(Vector2 position)
    {
        Vector3 spawnPos = new Vector3(position.x, position.y, fixedZPosition);
        return SpawnItemAtPosition3D(spawnPos);
    }
    
    /// <summary>
    /// 特定の座標にアイテムを配置（3D座標版）
    /// </summary>
    /// <param name="position">配置座標</param>
    /// <returns>配置されたアイテム</returns>
    public GameObject SpawnItemAtPosition3D(Vector3 position)
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0) return null;
        
        GameObject itemPrefab = GetRandomItemPrefab();
        GameObject spawnedItem = Instantiate(itemPrefab, position, Quaternion.identity);
        
        spawnedItems.Add(spawnedItem);
        
        if (showDebugLog)
        {
            Debug.Log($"アイテム '{itemPrefab.name}' を ({position.x}, {position.y}) に手動配置");
        }
        
        return spawnedItem;
    }
    
    /// <summary>
    /// 配置エリアを設定（2D）
    /// </summary>
    /// <param name="minPosition">最小座標（X, Y）</param>
    /// <param name="maxPosition">最大座標（X, Y）</param>
    public void SetSpawnArea(Vector2 minPosition, Vector2 maxPosition)
    {
        spawnAreaMin = minPosition;
        spawnAreaMax = maxPosition;
        
        if (showDebugLog)
        {
            Debug.Log($"配置エリアを設定: ({minPosition.x}, {minPosition.y}) ～ ({maxPosition.x}, {maxPosition.y})");
        }
    }
    
    /// <summary>
    /// 配置済みアイテム数を取得
    /// </summary>
    /// <returns>配置済みアイテム数</returns>
    public int GetSpawnedItemCount()
    {
        int count = 0;
        foreach (GameObject item in spawnedItems)
        {
            if (item != null)
            {
                count++;
            }
        }
        return count;
    }
    
    /// <summary>
    /// 画面内にアイテムを配置（カメラ範囲内）
    /// </summary>
    /// <param name="camera">対象のカメラ</param>
    /// <param name="margin">画面端からの余白</param>
    public void SpawnItemsInCameraView(Camera camera, float margin = 1f)
    {
        if (camera == null) camera = Camera.main;
        
        // カメラの画面範囲を取得
        Vector3 bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        
        // 余白を考慮して配置範囲を設定
        Vector2 newMin = new Vector2(bottomLeft.x + margin, bottomLeft.y + margin);
        Vector2 newMax = new Vector2(topRight.x - margin, topRight.y - margin);
        
        SetSpawnArea(newMin, newMax);
        SpawnRandomItems();
        
        if (showDebugLog)
        {
            Debug.Log("カメラ範囲内にアイテムを配置しました");
        }
    }
    
    void OnDrawGizmos()
    {
        if (!showGizmos) return;
        
        // 配置エリアを描画（2D矩形）
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((spawnAreaMin.x + spawnAreaMax.x) / 2, (spawnAreaMin.y + spawnAreaMax.y) / 2, fixedZPosition);
        Vector3 size = new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 0);
        Gizmos.DrawWireCube(center, size);
        
        // 配置済みアイテムの重複チェック範囲を描画
        Gizmos.color = Color.yellow;
        foreach (GameObject item in spawnedItems)
        {
            if (item != null)
            {
                Vector3 itemPos = new Vector3(item.transform.position.x, item.transform.position.y, fixedZPosition);
                Gizmos.DrawWireSphere(itemPos, overlapCheckRadius);
            }
        }
    }
}
