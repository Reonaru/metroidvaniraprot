using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour
{
    [Header("設定")]
    public CameraManager1 cameraManager;         // 💡 インスペクターでCameraManager1をD&D
    public float targetZoom = 2.0f;              // ズームイン先のカメラサイズ (例: 5.0→2.0)
    public Transform zoomTargetLocation;         // 💡 ズームの中心にしたいGameObject
    public bool resetOnExit = true;      
    private Playersc playersc;        // Triggerから出たらズームを元に戻すか？
    public float delayAfterzoom = 10f;
    
    [Header("ズーム維持時間")]
    public float minZoomDuration = 3.0f;
    
    private bool hasZoomed = false;

    void Start()
    {
        // CameraManager1の参照を確認
        if (cameraManager == null)
        {
            cameraManager = FindObjectOfType<CameraManager1>();
        }
        if (zoomTargetLocation == null)
        {
            Debug.LogError("Zoom Target Locationが設定されていません。");
        }

        playersc = FindObjectOfType<Playersc>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasZoomed)
        {

         // ★ プレイヤーの動きを停止
        if (playersc != null)
        {
            playersc.enabled = false;
        }

// ★ プレイヤーオブジェクトから Rigidbody2D を取得して速度をリセット
    Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.velocity = Vector2.zero;      // X軸・Y軸の速度をゼロに
        rb.angularVelocity = 0f;       // 回転速度をゼロに（念のため）
    }



            // ズームイン
            cameraManager.ZoomAndMoveTo(
                zoomTargetLocation.position, // ターゲットのX, Y座標
                targetZoom,
                delayAfterzoom
            );
            hasZoomed = true;
        }

    // ★ アニメーション完了 + 維持時間後にプレイヤーを再開するコルーチンを開始
            // ※ CameraManager1側でアニメーション時間を設定しているとして、ここでは維持時間のみを遅延させる
        //    StartCoroutine(ReEnablePlayerAfterDelay(minZoomDuration));


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && resetOnExit)
        {
            // ズームアウト (元のサイズに戻す)
            cameraManager.ResetZoom();
            hasZoomed = false;
        }
    }



    // ★ 追加するコルーチン：プレイヤーを再開するまでの時間を待つ
    private IEnumerator ReEnablePlayerAfterDelay(float delay)
    {
        // ズームアニメーションにかかる時間も待機時間に入れる場合は、
        // cameraManagerからその時間を取得してここに加算してください。
        
        yield return new WaitForSeconds(delay);

        // 遅延終了後、まだプレイヤーが停止状態であれば再開
        if (playersc != null && !playersc.enabled)
        {
            playersc.enabled = true;
        }
    }

}