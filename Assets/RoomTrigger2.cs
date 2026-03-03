using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger2 : MonoBehaviour
{
    public RoomManager roomManager;
    public RoomData nextRoomData; // ← これをインスペクターでセットする

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (roomManager.cameraManager.IsScrolling()) return;
            // スクロールを開始
            roomManager.StartScroll(nextRoomData);
        }
    }
}
