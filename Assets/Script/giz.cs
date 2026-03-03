using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giz : MonoBehaviour
{

    void OnDrawGizmos()
    {
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            Gizmos.color = Color.yellow;
            
            float height = cam.orthographicSize * 2;
            float width = height * cam.aspect;
            
            // 画面範囲を四角で表示
            Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
        }
    }
   
}
