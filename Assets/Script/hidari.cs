using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidari : MonoBehaviour
{
void Update()
{
            Camera cam = Camera.main;
        float leftEdge = cam.transform.position.x - (cam.orthographicSize * cam.aspect);
        transform.position = new Vector3(leftEdge-1, cam.transform.position.y, 0);

        
    }


void OnTriggerEnter2D(Collider2D other)
{

    if (other.CompareTag("Player"))
    {
  //      Camera.main.transform.position += new Vector3(8, 0, -10);
  //    other.transform.position += new Vector3(1, 0, 0);
  //    Debug.Log(transform.position);
            Camera cam = Camera.main;
            float screenWidth = cam.orthographicSize * 2 * cam.aspect;
            
            // 左に1画面分移動
            cam.transform.position -= new Vector3(screenWidth, 0, 0);
            
            // プレイヤーは左端に配置
            other.transform.position = new Vector3(
                cam.transform.position.x + screenWidth/2 - 2, 
                other.transform.position.y, 
                0
            );
    }
}

}
