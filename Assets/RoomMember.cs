using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMember : MonoBehaviour
{
    public string roomID;
    public string enemy_name;
    public List<GameObject> enemies = new List<GameObject>();
    private bool isCleared = false;

    void Start()
    {
        Enemy2D.OnAnyEnemyDeath += OnEnemyDied;
        // 子要素から敵を自動取得（手動セットでもOK）
        foreach (Transform t in transform.Find(enemy_name))
        {
            if (t.CompareTag("enemy"))
            {
            enemies.Add(t.gameObject);
        }
        }
    }

    // 敵が死ぬたびに呼ばれる
    public void OnEnemyDied(GameObject enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("敵が死んだ"  + enemy.name + "残りの敵の数: " + enemies.Count);
Debug.Log("敵が死んだ"  + enemy.name + "残りの敵の数: " + enemies.Count);
        if (enemies.Count <= 0 && !isCleared)
        {
            isCleared = true;
            // GameManagerに通知
            Gmanager.Instance.SetFlag(roomID + "_Clear", true);
        }
    }
}
