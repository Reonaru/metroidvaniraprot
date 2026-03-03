using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floar : MonoBehaviour
{
    [System.Serializable]
    public class Floor
    {
        public Vector3 cameraPosition;
        public string floorName;
    }
    
    public Floor[] floors;
    public Camera mainCamera;
    public GameObject player;
    public float transitionSpeed = 2f;
    
    public void MoveToFloor(int floorIndex)
    {
        if (floorIndex >= 0 && floorIndex < floors.Length)
        {
            StartCoroutine(TransitionToFloor(floors[floorIndex]));
        }
    }
    
    IEnumerator TransitionToFloor(Floor targetFloor)
    {
        Vector3 startPos = mainCamera.transform.position;
        float elapsedTime = 0;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetFloor.cameraPosition, elapsedTime);
            yield return null;
        }
    }
}
