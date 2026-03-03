using UnityEngine;
using System;

// プレイヤーが特定の場所に来たことを通知するクラス
public class BackgroundEventTrigger : MonoBehaviour
{
    // 💡 イベントの定義
    // static なので、シーンのどこからでもアクセスできる
    // delegate は関数の型、event はその関数を登録・解除するための仕組み
    public static event Action OnFlashEncounter; 

    // このTriggerはColliderにアタッチし、IsTriggerをONにする
    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーであることをタグで確認
        if (other.CompareTag("Player"))
        {
            // 💡 イベントの発行（購読者がいるかチェックして実行）
            if (OnFlashEncounter != null)
            {
                OnFlashEncounter.Invoke();
            }
            
            // 演出は一度きりにしたいので、Triggerを無効化する
            GetComponent<Collider2D>().enabled = false;
        }
    }
}