using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour
{
    public GameObject screenPrefab;
    public Vector2 screenSize = new Vector2(17.78f, 10f);
    
    void Start()
    {
        PlaceScreenGrid();
    }

    [ContextMenu("Place Screen Grid")]
    void PlaceScreenGrid()
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Vector3 position = new Vector3(x * screenSize.x, y * screenSize.y, 0);
                GameObject instance = Instantiate(screenPrefab);
                instance.transform.position = position; 
                 Debug.Log($"Created at: {position}");
            }
        }
    }
}

