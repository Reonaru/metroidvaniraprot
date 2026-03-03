using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map2 : MonoBehaviour
{
    [Header("タイル設定")]
    public GameObject tilePrefab;
    public int tilesPerRow = 10;     // 横に何個並べるか
    public float tileWidth = 1f;     // タイル1個の幅
    public float groundHeight = -4f; // 画面下からの高さ

        [Header("床判定設定")]
    public bool addColliders = true;
    
    void Start()
    {
        GenerateGroundTiles();
    }
    
    void GenerateGroundTiles()
    {
        Vector3 screenCenter = transform.position;
        
        // 開始位置（左端）を計算
        float startX = screenCenter.x - (tilesPerRow * tileWidth) / 2f;
        float groundY = screenCenter.y + groundHeight;
        
        for (int i = 0; i < tilesPerRow; i++)
        {
            Vector3 tilePosition = new Vector3(
                startX + (i * tileWidth),
                groundY,
                0
            );
            
            GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
            tile.transform.parent = transform; // 画面の子オブジェクトにする
            tile.name = $"GroundTile_{i}";     // 名前をつける

            if (addColliders)
            {
                AddGroundCollider(tile);
            }
        }
    }

        void AddGroundCollider(GameObject tile)
    {
        // BoxCollider2Dを追加
        BoxCollider2D collider = tile.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = tile.AddComponent<BoxCollider2D>();
        }
        
        // サイズをタイルに合わせる
        collider.size = new Vector2(tileWidth, tileWidth);
        collider.isTrigger = false; // 床なのでTriggerではない
    }
}
