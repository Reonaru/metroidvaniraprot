using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    // 💡 RoomManagerが設定する (自動生成時に設定される)
    public RoomManager roomManager;
    public RoomData currentRoomData; // Triggerがある部屋の情報
    public RoomData nextRoomData;    // Triggerを越えた先の部屋の情報


    // プレイヤーの左右どちらからの接触か？
    private bool PlayerIsMovingRight(Collider2D other)
    {
        return other.transform.position.x < transform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomData targetRoom;


            
            // 💡 左右の方向判定
            if (PlayerIsMovingRight(other))
            {
                // 右へ移動 -> 次の部屋へ
                targetRoom = nextRoomData;
                Debug.Log("どっち？next");
            }
            else
            {
                // 左へ移動 -> 現在の部屋（元の部屋）へ戻る
                targetRoom = currentRoomData;
            }
            Debug.Log($"targetRoom.roomID{targetRoom.roomID}");
            Debug.Log($"nextRoomData.roomID:{nextRoomData.roomID}");
            // 1. スクロールを開始
            roomManager.StartScroll(targetRoom);
            
            // 2. 部屋の境界とIDを更新
            // ※ 通常はスクロールが完了した後に実行すべきですが、ここでは即座に適用
          roomManager.SetCurrentRoom(targetRoom);
            Debug.Log($"SetCurrentRoom:{targetRoom.roomID}");

            // 3. 処理完了後、このTriggerを無効化
            gameObject.SetActive(false); 
        }
    }
}