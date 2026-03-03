using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random2 : MonoBehaviour
{
    [Header("アイテム設定")]
    [SerializeField] private GameObject[] itemPrefabs;  // 配置するアイテムのプリファブ
    [SerializeField] private int maxItemCount = 5;      // 最大同時表示数
    
    [Header("配置範囲設定（2D）")]
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10, -5);  // 配置範囲の最小座標（X, Y）
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(10, 5);    // 配置範囲の最大座標（X, Y）
    [SerializeField] private float fixedZPosition = 0f;                    // 固定するZ座標
    
    [Header("タイミング設定")]
    [SerializeField] private float spawnInterval = 5f;       // アイテム表示間隔（秒）
    [SerializeField] private float itemLifetime = 5f;       // アイテムの表示時間（秒）
    [SerializeField] private bool autoStart = true;         // 自動開始
    
    [Header("配置条件")]
    [SerializeField] private LayerMask obstacleLayerMask = 0;    // 障害物のレイヤー
    [SerializeField] private float minDistanceBetweenItems = 2f; // アイテム間の最小距離
    [SerializeField] private float overlapCheckRadius = 1f;     // 重複チェックの半径
    [SerializeField] private int maxSpawnAttempts = 50;         // 配置試行回数の上限
    
    [Header("配置方法")]
    [SerializeField] private bool avoidObstacles = true;        // 障害物を避ける
    
    [Header("デバッグ")]
    [SerializeField] private bool showDebugLog = true;
    [SerializeField] private bool showGizmos = true;
    
    private List<GameObject> activeItems = new List<GameObject>();  // 現在表示中のアイテム
    private Queue<GameObject> itemQueue = new Queue<GameObject>();  // 表示順序を管理するキュー
    private Coroutine spawnCoroutine;
    private bool isSpawning = false;
    
    void Start()
    {
        if (autoStart)
        {
            StartSpawning();
        }
    }
    
    /// <summary>
    /// アイテムの段階的表示を開始
    /// </summary>
    public void StartSpawning()
    {
        if (isSpawning)
        {
            if (showDebugLog)
            {
                Debug.Log("既にスポーン中です");
            }
            return;
        }
        
        if (itemPrefabs == null || itemPrefabs.Length == 0)
        {
            Debug.LogWarning("アイテムプリファブが設定されていません");
            return;
        }
        
        isSpawning = true;
        spawnCoroutine = StartCoroutine(SpawnItemsOverTime());
        
        if (showDebugLog)
        {
            Debug.Log("アイテムの段階的表示を開始しました");
        }
    }
    
    /// <summary>
    /// アイテムの段階的表示を停止
    /// </summary>
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        
        isSpawning = false;
        
        if (showDebugLog)
        {
            Debug.Log("アイテムの段階的表示を停止しました");
        }
    }
    
    /// <summary>
    /// 時間差でアイテムを表示するコルーチン
    /// </summary>
    private IEnumerator SpawnItemsOverTime()
    {
        while (isSpawning)
        {
            // 上限に達していない場合のみ新しいアイテムを表示
            if (activeItems.Count < maxItemCount)
            {
                SpawnNewItem();
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    /// <summary>
    /// 新しいアイテムを1つ表示
    /// </summary>
    private void SpawnNewItem()
    {
        Vector3 spawnPosition = GetValidSpawnPosition();
        
        if (spawnPosition != Vector3.zero)
        {
            GameObject itemPrefab = GetRandomItemPrefab();
            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            
            activeItems.Add(newItem);
            itemQueue.Enqueue(newItem);
            
            // 指定時間後にアイテムを削除
            StartCoroutine(RemoveItemAfterTime(newItem, itemLifetime));
            
            if (showDebugLog)
            {
                Debug.Log($"アイテム '{itemPrefab.name}' を ({spawnPosition.x:F1}, {spawnPosition.y:F1}) に表示 (現在: {activeItems.Count}/{maxItemCount})");
            }
        }
        else
        {
            if (showDebugLog)
            {
                Debug.LogWarning("有効な配置位置が見つかりませんでした");
            }
        }
    }
    
    /// <summary>
    /// 指定時間後にアイテムを削除するコルーチン
    /// </summary>
    /// <param name="item">削除するアイテム</param>
    /// <param name="delay">削除までの時間</param>
    private IEnumerator RemoveItemAfterTime(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (item != null)
        {
            RemoveItem(item);
        }
    }
    
    /// <summary>
    /// アイテムを削除
    /// </summary>
    /// <param name="item">削除するアイテム</param>
    private void RemoveItem(GameObject item)
    {
        if (item != null)
        {
            activeItems.Remove(item);
            
            if (showDebugLog)
            {
                Debug.Log($"アイテム '{item.name}' を削除 (残り: {activeItems.Count}/{maxItemCount})");
            }
            
            Destroy(item);
        }
    }
    
    /// <summary>
    /// 有効な配置位置を取得
    /// </summary>
    /// <returns>有効な座標（見つからない場合はVector3.zero）</returns>
    private Vector3 GetValidSpawnPosition()
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector3 candidatePosition = GetRandomSpawnPosition();
            
            if (IsValidSpawnPosition(candidatePosition))
            {
                return candidatePosition;
            }
        }
        
        return Vector3.zero; // 有効な位置が見つからない
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
        
        // 他のアクティブなアイテムとの距離チェック
        foreach (GameObject item in activeItems)
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
    /// 全てのアクティブアイテムを削除
    /// </summary>
    public void ClearAllItems()
    {
        StopSpawning();
        
        foreach (GameObject item in activeItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        
        activeItems.Clear();
        itemQueue.Clear();
        
        if (showDebugLog)
        {
            Debug.Log("全てのアイテムを削除しました");
        }
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
    /// スポーン間隔を変更
    /// </summary>
    /// <param name="newInterval">新しい間隔（秒）</param>
    public void SetSpawnInterval(float newInterval)
    {
        spawnInterval = newInterval;
        
        if (showDebugLog)
        {
            Debug.Log($"スポーン間隔を {newInterval} 秒に変更");
        }
    }
    
    /// <summary>
    /// アイテムの表示時間を変更
    /// </summary>
    /// <param name="newLifetime">新しい表示時間（秒）</param>
    public void SetItemLifetime(float newLifetime)
    {
        itemLifetime = newLifetime;
        
        if (showDebugLog)
        {
            Debug.Log($"アイテム表示時間を {newLifetime} 秒に変更");
        }
    }
    
    /// <summary>
    /// 現在のアクティブアイテム数を取得
    /// </summary>
    /// <returns>アクティブアイテム数</returns>
    public int GetActiveItemCount()
    {
        // nullチェックをして有効なアイテムのみカウント
        int count = 0;
        foreach (GameObject item in activeItems)
        {
            if (item != null)
            {
                count++;
            }
        }
        return count;
    }
    
    /// <summary>
    /// スポーン中かどうかを取得
    /// </summary>
    /// <returns>スポーン中の場合はtrue</returns>
    public bool IsSpawning()
    {
        return isSpawning;
    }
    
    void OnDrawGizmos()
    {
        if (!showGizmos) return;
        
        // 配置エリアを描画（2D矩形）
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((spawnAreaMin.x + spawnAreaMax.x) / 2, (spawnAreaMin.y + spawnAreaMax.y) / 2, fixedZPosition);
        Vector3 size = new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 0);
        Gizmos.DrawWireCube(center, size);
        
        // アクティブアイテムの重複チェック範囲を描画
        Gizmos.color = Color.yellow;
        foreach (GameObject item in activeItems)
        {
            if (item != null)
            {
                Vector3 itemPos = new Vector3(item.transform.position.x, item.transform.position.y, fixedZPosition);
                Gizmos.DrawWireSphere(itemPos, overlapCheckRadius);
            }
        }
        
        // アクティブアイテムの表示
        Gizmos.color = Color.red;
        foreach (GameObject item in activeItems)
        {
            if (item != null)
            {
                Vector3 itemPos = new Vector3(item.transform.position.x, item.transform.position.y, fixedZPosition);
                Gizmos.DrawWireSphere(itemPos, 0.2f);
            }
        }
    }
}