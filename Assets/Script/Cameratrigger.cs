using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameratrigger : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 nextFloorCameraPosition; // 次のフロアのカメラ位置
    public Vector3 playerSpawnPosition;     // プレイヤーの移動先位置
    public float transitionSpeed = 2f;      // 移動速度
    
    private bool isTransitioning = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(MoveToNextFloor(other.gameObject));
        }
    }
    
    IEnumerator MoveToNextFloor(GameObject player)
    {
        isTransitioning = true;
        
        // プレイヤーを新しい位置に移動
        player.transform.position = playerSpawnPosition;
        
        // カメラを新しい位置にスムーズに移動
        Vector3 startPos = mainCamera.transform.position;
        float elapsedTime = 0;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, nextFloorCameraPosition, elapsedTime);
            yield return null;
        }
        
        mainCamera.transform.position = nextFloorCameraPosition;
        isTransitioning = false;
    }
}

