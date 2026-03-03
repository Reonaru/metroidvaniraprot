using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomState : MonoBehaviour
{
    // 💡 現在プレイヤーがいる部屋のRoomID
    public int currentRoomID { get; private set; } 

    // 外部からIDを更新するためのメソッド
    public void SetCurrentRoomID(int newID)
    {
        // 部屋のIDを更新する
        currentRoomID = newID;
        Debug.Log($"プレイヤーの部屋IDが更新されました: {currentRoomID}");

        // ID更新に伴い、特定の処理をここに追加できる
    }

    // 外部からIDを読み取って判定に使う
    public bool IsInRoom(int targetID)
    {
        return currentRoomID == targetID;
    }
}