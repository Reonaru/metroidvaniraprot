using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StgCameraManager : MonoBehaviour
{
    public GameObject target; // プレイヤー
    public float leftLimit = 0f;    // カメラの左端制限
    public float rightLimit = 20f;  // カメラの右端制限
    public float topLimit = 10f;    // カメラの上端制限（必要に応じて）
    public float bottomLimit = 0f;  // カメラの下端制限（必要に応じて）

    
    Vector3 fixedPos;

    void Start()
    {
        fixedPos = new Vector3(0, 0, -10);
        Camera.main.gameObject.transform.position = fixedPos;   
    }

    void Update()
    {
        if (target == null) return;
        
        Vector3 cameraPos = target.transform.position;
        
        // 横方向の制限
        if (target.transform.position.x < leftLimit)
        {
            cameraPos.x = leftLimit;
        }
        else if (target.transform.position.x > rightLimit)
        {
            cameraPos.x = rightLimit;
        }
        
        // 縦方向の制限（コメントアウトしてあった部分を整理）
        if (target.transform.position.y < bottomLimit)
        {
            cameraPos.y = bottomLimit;
        }
        else if (target.transform.position.y > topLimit)
        {
            cameraPos.y = topLimit;
        }
        
        // Z座標は固定
        cameraPos.z = -10;
        Camera.main.gameObject.transform.position = cameraPos;
    }





    public void MoveScreen(float moveX, float moveY = 0)
    {

    //  float newLeftLimit = playerPos.x;
//    Vector3 cameraPos = Camera.main.gameObject.transform.position;
    Camera cameraPos = Camera.main;
    float cameraHeight = cameraPos.orthographicSize * 2;
    float cameraWidth = cameraHeight * cameraPos.aspect;
    float leftX = cameraPos.transform.position.x - cameraWidth / 2;

    Vector3 leftEdge = new Vector3(leftX, cameraPos.transform.position.y, 0);


    Vector3 playerPos = target.transform.position;
    playerPos.x = leftEdge.x;
    target.transform.position = playerPos;


        // カメラの制限範囲を移動
        leftLimit += moveX;
        rightLimit += moveX;
        topLimit += moveY;
        bottomLimit += moveY;
        
 //   Camera.main.transform.position += new Vector3(moveX, moveY, 0);

        // カメラも同時に移動


    Debug.Log($"playerPos:{playerPos}");
    Debug.Log($"LeftLimit:{leftLimit}");
    Debug.Log($"cameraPos:{Camera.main.transform.position.x}");

    }


}