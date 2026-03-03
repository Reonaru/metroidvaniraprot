using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class up : MonoBehaviour
{
    void Update()
    {
        Camera cam = Camera.main;
        // 画面の上端に配置
        float topEdge = cam.transform.position.y + cam.orthographicSize;
        transform.position = new Vector3(cam.transform.position.x, topEdge+1, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Camera cam = Camera.main;
            float screenHeight = cam.orthographicSize * 2; // 画面の高さ
            
            // 上に1画面分移動
            cam.transform.position += new Vector3(0, screenHeight, 0);
            
            // プレイヤーは下端に配置
            other.transform.position = new Vector3(
                other.transform.position.x,
                cam.transform.position.y - screenHeight/2 + 2,
                0
            );
        }
    }
}
