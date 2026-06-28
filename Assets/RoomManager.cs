using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;


    public enum ScrollDirection { Right, Left, Up, Down }

public class RoomManager : MonoBehaviour
{
    private ScrollDirection moveDirection;

    public Playersc player;
    [Header("設定")]
    // 💡 シーン内の全ての部屋のデータをInspectorで設定
    public RoomData[] allRoomData;
    
    [Header("参照")]
    public CameraManager1 cameraManager;
    
    // 💡 現在の部屋のIDを保持するフラグ
    public int currentRoomID { get; private set; } = 0; 
    
    // 💡 TriggerのPrefabや生成対象 (ここではBox Colliderを持つ空のオブジェクトを想定)
    public GameObject triggerPrefab; 

    void Awake()
    {
        // 参照の確認
        if (cameraManager == null)
            cameraManager = FindObjectOfType<CameraManager1>();

        if (allRoomData.Length == 0)
        {
            Debug.LogError("RoomDataが設定されていません！");
            return;
        }

        // 💡 最初の部屋の境界とカメラ位置を設定
         SetCurrentRoom(allRoomData[0], false);


        // 💡 全てのTriggerを自動生成
//        GenerateRoomTriggers();
    }

    void Update()
    {
        // デバッグ用：数字キー（1～8）でRoom を直接ジャンプ
        for (int i = 1; i <= 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                Debug.Log($"キー {i} 押下。allRoomData.Length={allRoomData.Length}, 要求index={i-1}");
                if (i - 1 < allRoomData.Length)
                {
                    RoomData target = allRoomData[i - 1];
                    Debug.Log($"対象RoomData: {(target != null ? target.name : "NULL")}");
                    if (target != null)
                    {
                        JumpToRoom(target);
                    }
                }
                else
                {
                    Debug.LogWarning($"Room {i} はallRoomDataに存在しません");
                }
            }
        }
    }



    /// <summary>
    /// 💡 Triggerから呼ばれ、部屋の境界とIDを更新する
    /// </summary>
public void SetCurrentRoom(RoomData newRoom, bool shouldOffsetPlayer = false)
    {
        Debug.Log($"<color=red>SetCurrentRoom呼出:</color> Room={newRoom.name}, Offset={shouldOffsetPlayer}, TargetY={newRoom.YBoundary}");

if (shouldOffsetPlayer)
    {
        float offset = 3.0f; 
// Room 内の押し出し判定に追記
if (moveDirection == ScrollDirection.Right) player.transform.position += new Vector3(offset, 0, 0);
else if (moveDirection == ScrollDirection.Left) player.transform.position -= new Vector3(offset, 0, 0);
else if (moveDirection == ScrollDirection.Up) player.transform.position += new Vector3(0, offset, 0);    // 💡 追加
else if (moveDirection == ScrollDirection.Down) player.transform.position -= new Vector3(0, offset, 0);  // 💡 追加
    }

// 💡 プレイヤーを Trigger から引き離す「強制移動」
    // 例：右移動なら少し右へ、左移動なら少し左へ
        if (cameraManager != null)
        {
            // 💡 RoomDataから直接境界線を設定
            cameraManager.SetBounds(newRoom);
        }
        
        // 💡 部屋IDフラグを更新
        currentRoomID = newRoom.roomID;
        Debug.Log($"現在の部屋: Room ID {currentRoomID}, X追従範囲: {newRoom.minXBoundary} ~ {newRoom.maxXBoundary}");

        // 💡 Room11に入ったら敵スポーナーを開始
        if (newRoom.roomID == 11)
        {
            GameObject spawnerObj = GameObject.Find("Room11_EnemySpawner");
            if (spawnerObj != null)
            {
                EnemySpawner spawner = spawnerObj.GetComponent<EnemySpawner>();
                if (spawner != null)
                {
                    spawner.StartSpawning();
                }
            }
        }

        // 💡 Room8に入ったら進入イベント実行
        if (newRoom.roomID == 8)
        {
            GameObject dialogueObj = GameObject.Find("Room8_DialogueController");
            if (dialogueObj != null)
            {
                Room8Dialogue dialogue = dialogueObj.GetComponent<Room8Dialogue>();
                if (dialogue != null)
                {
                    dialogue.OnEnterRoom8();
                }
            }
        }
    }
    




    /// <summary>
    /// 💡 Triggerから呼ばれ、カメラのスクロールを開始する
    /// </summary>
public void StartScroll(RoomData targetRoom)
{
    if (cameraManager.IsScrolling()) return;

    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
    if (rb != null) rb.simulated = false;

    Vector3 camPos = cameraManager.transform.position;

    // 💡 スクロール方向をフラグで明示的に判定
    float finalX = camPos.x;
    float finalY = camPos.y;

    float diffX = targetRoom.minXBoundary - camPos.x;
    float diffY = targetRoom.YBoundary - camPos.y;

    if (targetRoom.scrollType == RoomData.ScrollType.Vertical)
    {
        // 縦スクロール：Y を目標値に移動、X は範囲内に調整
        moveDirection = (diffY > 0) ? ScrollDirection.Up : ScrollDirection.Down;
        finalY = targetRoom.YBoundary;
        finalX = Mathf.Clamp(camPos.x, targetRoom.minXBoundary, targetRoom.maxXBoundary);
    }
    else
    {
        // 横スクロール：X を移動、Y は固定
        if (diffX > 0) {
            moveDirection = ScrollDirection.Right;
            finalX = targetRoom.minXBoundary;
        } else {
            moveDirection = ScrollDirection.Left;
            finalX = targetRoom.maxXBoundary;
        }
    }

    Vector3 targetPos = new Vector3(finalX, finalY, camPos.z);

    cameraManager.ScrollTo(
    targetPos, 
    targetRoom.minXBoundary, 
    targetRoom.maxXBoundary, 
    targetRoom.maxYBoundary, () => {
　　　　　　　// 🚩 追加2: 終わったら物理を戻す
            if (rb != null) rb.simulated = true;

        SetCurrentRoom(targetRoom, true);
    });
}

    void JumpToRoom(RoomData targetRoom)
    {
        // プレイヤーを対応するRoom の中央に配置
        float centerX = (targetRoom.minXBoundary + targetRoom.maxXBoundary) / 2f;
        player.transform.position = new Vector3(centerX, 0, 0);

        // Room を直接設定（スクロール無し）
        SetCurrentRoom(targetRoom, false);

        Debug.Log($"<color=green>デバッグ：Room {targetRoom.roomID} にジャンプしました</color>");
    }

public void ResetRoom()
{
    SetCurrentRoom(allRoomData[0], false); // 最初の部屋に戻す
}


public RoomData GetRoomByPosition(Vector3 playerPos)
{
    foreach (RoomData room in allRoomData)
    {
        if (playerPos.x >= room.minXBoundary && playerPos.x <= room.maxXBoundary &&
            playerPos.y >= room.YBoundary    && playerPos.y <= room.maxYBoundary)
        {
            return room;
        }
    }
    return allRoomData[0]; // 見つからなければRoom1
}



}
