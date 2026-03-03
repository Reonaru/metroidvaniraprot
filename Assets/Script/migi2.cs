using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class migi2 : MonoBehaviour
{
    void Update()
    {
        // カメラの右端に常に配置
        Camera cam = Camera.main;
        float rightEdge = cam.transform.position.x + (cam.orthographicSize * cam.aspect);
        transform.position = new Vector3(rightEdge, cam.transform.position.y, 0);
    }
}
