using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map3 : MonoBehaviour
{
    [Header("タイル設定")]
    public GameObject tilePrefab;
    public float tileSize = 1f;

        private GameObject player;
    
        private Rigidbody2D playerRigidbody;
    [Header("画面サイズ")]
    public int screenWidthTiles = 20;  // 画面の横タイル数
    public int screenHeightTiles = 12; // 画面の縦タイル数
    
    [Header("レイアウト設定")]
    public bool createFloor = true;
    public bool createCeiling = false;
    public bool createLeftWall = false;
    public bool createRightWall = false;
    public bool createPlatforms = false;
    
    [Header("詳細設定")]
    public int floorThickness = 2;    // 床の厚さ
    public int ceilingThickness = 1;  // 天井の厚さ
    public int wallThickness = 1;     // 壁の厚さ

    public float delayTime = 10f;
    
    void Start()
    {
        GenerateScreenLayout();

     player = GameObject.FindWithTag("Player");
    if (player != null)
    {
                    playerRigidbody = player.GetComponent<Rigidbody2D>();

            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = 0;
                playerRigidbody.velocity = Vector2.zero; // 速度もリセット
            }


        player.SetActive(false); // プレイヤーを一時的に無効化
    }

    // タイル生成後にプレイヤーを復活
    Invoke("RestorePlayer", 5f);
    
    }

void RestorePlayer()
{
    if (player != null)
    {
        player.transform.position = new Vector3(0, -1, 0);
        player.SetActive(true); // 復活
        Debug.Log("動いてる？");
                    Invoke("RestoreGravity", 10f);
    }
}

    void RestoreGravity()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.gravityScale = 0; // 重力を戻す
        }
    }


    void GenerateScreenLayout()
    {
        Vector3 screenCenter = transform.position;
        Vector3 startPos = screenCenter - new Vector3(
            (screenWidthTiles * tileSize) / 2f,
            (screenHeightTiles * tileSize) / 2f,
            0
        );
        
        // 床を作成
        if (createFloor)
        {
            CreateFloor(startPos);
        }
        
        // 天井を作成
        if (createCeiling)
        {
            CreateCeiling(startPos);
        }
        
        // 左壁を作成
        if (createLeftWall)
        {
            CreateLeftWall(startPos);
        }
        
        // 右壁を作成
        if (createRightWall)
        {
            CreateRightWall(startPos);
        }
        
        // 全体にコライダーを追加
        CreateScreenCollider();
    }
    
    void CreateFloor(Vector3 startPos)
    {
        for (int x = 0; x < screenWidthTiles; x++)
        {
            for (int y = 0; y < floorThickness; y++)
            {
                Vector3 tilePos = startPos + new Vector3(x * tileSize, y * tileSize, 0);
                CreateTile(tilePos, "Floor");
            }
        }
    }
    
    void CreateCeiling(Vector3 startPos)
    {
        int ceilingStartY = screenHeightTiles - ceilingThickness;
        for (int x = 0; x < screenWidthTiles; x++)
        {
            for (int y = ceilingStartY; y < screenHeightTiles; y++)
            {
                Vector3 tilePos = startPos + new Vector3(x * tileSize, y * tileSize, 0);
                CreateTile(tilePos, "Ceiling");
            }
        }
    }
    
    void CreateLeftWall(Vector3 startPos)
    {
            Debug.Log($"左壁作成開始 - startPos: {startPos}");
    Debug.Log($"wallThickness: {wallThickness}, screenHeightTiles: {screenHeightTiles}");
    
        for (int x = 0; x < wallThickness; x++)
        {
            for (int y = 0; y < screenHeightTiles; y++)
            {
                Vector3 tilePos = startPos + new Vector3(-(x * tileSize), y * tileSize, 0);
                CreateTile(tilePos, "LeftWall");
            }
        }
    }
    
    void CreateRightWall(Vector3 startPos)
    {
        int wallStartX = screenWidthTiles - wallThickness;
        for (int x = wallStartX; x < screenWidthTiles; x++)
        {
            for (int y = 0; y < screenHeightTiles; y++)
            {
                Vector3 tilePos = startPos + new Vector3(x * tileSize, y * tileSize, 0);
                CreateTile(tilePos, "RightWall");
            }
        }
    }
    
    void CreateTile(Vector3 position, string tileName)
    {
        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
        tile.transform.parent = transform;
        tile.name = tileName;
    }
    
    void CreateScreenCollider()
    {
        // 画面全体のコライダーを作成
        GameObject colliderObj = new GameObject("ScreenCollider");
        colliderObj.transform.parent = transform;
        colliderObj.transform.position = transform.position;
        
        BoxCollider2D collider = colliderObj.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(screenWidthTiles * tileSize, screenHeightTiles * tileSize);
        collider.isTrigger = false;
    }
}