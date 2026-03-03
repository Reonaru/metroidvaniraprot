using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map4 : MonoBehaviour
{
    [Header("画面設定")]
    public GameObject screenLayoutPrefab; // ScreenLayoutGenerator付きのPrefab
    public Vector2 screenSize = new Vector2(17.78f, 10f);
    public int mapWidth = 10;
    public int mapHeight = 6;
    
    void Start()
    {
        GenerateAllScreens();
        
    }
    
    void GenerateAllScreens()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 screenPosition = new Vector3(
                    x * screenSize.x,
                    y * screenSize.y,
                    0
                );
                
                GameObject screen = Instantiate(screenLayoutPrefab, screenPosition, Quaternion.identity);
                screen.name = $"Screen_{x}_{y}";
                screen.transform.parent = transform;
            }
        }
    }



}
