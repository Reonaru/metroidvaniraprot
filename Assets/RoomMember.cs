using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMember : MonoBehaviour
{
    public string roomID;
    public string enemy_name;
    public List<GameObject> enemies = new List<GameObject>();
    private bool isCleared = false;

    void Start()
    {
        Enemy2D.OnAnyEnemyDeath += OnEnemyDied;
        // 子要素から敵を自動取得（手動セットでもOK）
        foreach (Transform t in transform.Find(enemy_name))
        {
            if (t.CompareTag("enemy"))
            {
            enemies.Add(t.gameObject);
        }
        }
    }

    // 敵が死ぬたびに呼ばれる
    public void OnEnemyDied(GameObject enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("敵が死んだ"  + enemy.name + "残りの敵の数: " + enemies.Count);
Debug.Log("敵が死んだ"  + enemy.name + "残りの敵の数: " + enemies.Count);
        if (enemies.Count <= 0 && !isCleared)
        {
            isCleared = true;
            // GameManagerに通知
            Gmanager.Instance.SetFlag(roomID + "_Clear", true);

    if (roomID == "Room4")
    {
        Gmanager.Instance.SetCheckPoint(1); 
    }
    else if (roomID == "Room5")
    {
        Gmanager.Instance.SetCheckPoint(2); // 別のチェックポイントが必要なら
    }

            if (roomID == "Room4"){

            CameraManager1 cam = FindObjectOfType<CameraManager1>();
            RoomManager rm = FindObjectOfType<RoomManager>();

            RoomData clearedRoom = rm.allRoomData[rm.currentRoomID - 1];

            Vector3 blockPos = new Vector3(42.84f, 0.34f, -10f);
            cam.ZoomAndMoveTo(blockPos, 4f, 2f);
            StartCoroutine(AfterCutscene(2f + 1f, rm, cam, clearedRoom));
            }
        }
    }

    IEnumerator AfterCutscene(float seconds, RoomManager rm, CameraManager1 cam, RoomData clearedRoom)
{
    yield return new WaitForSeconds(seconds);
    
    // ズームを戻す
//    cam.ResetZoom();
 cam.ResetZoom();    
    RoomData currentRoom = rm.allRoomData[rm.currentRoomID];
    Debug.Log($"minX: {currentRoom.minXBoundary} maxX: {currentRoom.maxXBoundary} プレイヤーX: {rm.player.transform.position.x}");
    

    // 今いる部屋の境界を再設定

    cam.UpdateCameraBounds(
        clearedRoom.minXBoundary,
        clearedRoom.maxXBoundary,
        clearedRoom.YBoundary,
        clearedRoom.maxYBoundary
    );

    cam.ForceStopScrolling();

    Debug.Log("isScrolling後: " + cam.IsScrolling());
Debug.Log("プレイヤー座標: " + rm.player.transform.position);
Debug.Log("playerオブジェクト名: " + rm.player.gameObject.name);
        // カメラをプレイヤーに強制的に戻す
    cam.transform.position = new Vector3(
        rm.player.transform.position.x,
        rm.player.transform.position.y,
        -10f
    );


    Debug.Log("カメラ移動後: " + cam.transform.position);
    
    // orthographicSizeを直接戻す
 //   Camera.main.orthographicSize = cam.defaultZoom;
}
}
