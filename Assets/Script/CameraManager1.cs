using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager1 : MonoBehaviour
{

    // --- 設定値 ---
    [Header("スクロール")]
    public float scrollDuration = 1f;
    public float delayAfterZoom = 3f;

    [Header("追従")]
    public Transform target;
    public float followSmoothSpeed = 0.125f;

    [Header("境界制限")]
    public float minXLimit = 0f;
    public float maxXStopLimit = 10f;
    public float minYLimit; // 以前の currentFixedY の役割
    public float maxYLimit; // 🚩 新設：これがないと上に動けない
    
    
    private Vector3 initialminXLimit;
    private Vector3 initiolmaxXStopLimit;


    private Vector3 followVelocity;
    private bool isScrolling = false;

    public GameObject scrol;
    public RoomManager roommanager;

    [SerializeField] RoomData testdata;

    [Header("ズーム/スクロール設定")]
    public float defaultZoom = 5f;       // 通常時のカメラサイズ (Unityで設定した値)

    // ★ プレイヤーのリトライに備えて、カメラの初期位置を記憶する
    private Vector3 initialPosition;
    private Vector3 initialScrolPosition;
    private float initialMinXLimit;
    private float initialMaxXStopLimit;
    private float initialMinYLimit;
    private float initialMaxYLimit;


public float HalfWidth
    {
        get { return Camera.main.orthographicSize * Camera.main.aspect; }
    }

void Start()
    {
        // 1. カメラの初期位置を記憶
        initialPosition = transform.position;
        Debug.Log($"📷 カメラの初期位置: {initialPosition}");

        // 2. カメラ境界線の初期値を記憶
        initialMinXLimit = minXLimit;
        initialMaxXStopLimit = maxXStopLimit;
        initialMinYLimit = minYLimit;
        initialMaxYLimit = maxYLimit;
        Debug.Log($"📷 カメラ境界線初期値: minX={initialMinXLimit}, maxX={initialMaxXStopLimit}, minY={initialMinYLimit}, maxY={initialMaxYLimit}");

        // 3. scrolオブジェクトの初期位置を記憶
        GameObject scrol = GameObject.Find("scrol");

        if (scrol != null)
        {
            initialScrolPosition = scrol.transform.position;
        }
        else
        {
            Debug.LogError("scrolオブジェクトが見つかりませんでした。");
        }

        // ...
    }


    // --- カメラ移動（画面単位スクロール） ---
 public void ScrollTo(Vector3 targetPos, float newMinXLimit, float newMaxXLimit, float newMaxYLimit, Action onComplete)
{
    if (!isScrolling)
        StartCoroutine(ScrollCamera(targetPos, newMinXLimit, newMaxXLimit, newMaxYLimit, onComplete));
}
 
private IEnumerator ScrollCamera(Vector3 targetPos, float newMinXLimit, float newMaxXLimit, float newMaxYLimit, Action onComplete)
{
    isScrolling = true;
    Vector3 startPos = transform.position;
    Camera cam = Camera.main; 

    // 💡 修正1: 必要な変数は最初に全て計算しておく
    float halfWidth = cam.orthographicSize * cam.aspect;
    float halfHeight = cam.orthographicSize; 
    
    float t = 0f;
    Debug.Log("Start Scroll");

    while (t < scrollDuration)
    {
        transform.position = Vector3.Lerp(startPos, targetPos, t / scrollDuration);
        t += Time.deltaTime;
        yield return null;
    }

    transform.position = targetPos;

    // 💡 修正2: 引数と計算済みの変数を使って境界を更新
    minXLimit = newMinXLimit + halfWidth;
    maxXStopLimit = newMaxXLimit - halfWidth;
    
    minYLimit = targetPos.y; 
    // 🚩 目的地(targetPos.y)が部屋の床、newMaxYLimitが天井の高さ
    maxYLimit = newMaxYLimit - halfHeight; 

    currentVelocity = Vector3.zero;

    onComplete?.Invoke(); 
    isScrolling = false;
    Debug.Log($"Stop Scroll {isScrolling}");    
}
 
public void UpdateCameraBounds(float newMinX, float newMaxX, float newMinY, float newMaxY)
{
    Camera cam = Camera.main; 
    float halfWidth = cam.orthographicSize * cam.aspect;
    float halfHeight = cam.orthographicSize; // 💡 カメラの高さの半分

    // 🚩 左右の計算を「同じルール」で統一（これで戻る現象を防ぐ）
    minXLimit = newMinX + halfWidth;
    maxXStopLimit = newMaxX - halfWidth;

// 🚩 Y軸もカメラのサイズ（半分）を考慮して制限をかける
    minYLimit = newMinY;
    maxYLimit = newMaxY; // 天井からカメラ半分を引いた位置が限界

    currentVelocity = Vector3.zero;
    
}


// ★ リトライ用に外部から呼ばれるリセット関数
    public void ResetCameraForRetry()
    {
        // 1. カメラを初期位置に戻す
        transform.position = initialPosition;

        // 2. スクロール制限変数を初期値に戻す
        minXLimit = initialMinXLimit;
        maxXStopLimit = initialMaxXStopLimit;
        minYLimit = initialMinYLimit;
        maxYLimit = initialMaxYLimit;
        Debug.Log($"📷 カメラ境界線をリセット: minX={minXLimit}, maxX={maxXStopLimit}, minY={minYLimit}, maxY={maxYLimit}");

        // 3. 慣性（SmoothDampなど）とフラグをリセット
        currentVelocity = Vector3.zero;
        isScrolling = false;

        // 4. (あれば)スクロールを制御しているオブジェクト（例: "scrol"）も初期位置に戻す
        // ここはプロジェクトの構造によりますが、必要であれば追加します
        GameObject scrol = GameObject.Find("scrol");
        if (scrol != null)
        {
scrol.transform.position = initialScrolPosition;
        }
    }


    public bool IsScrolling()
    {
        return isScrolling;
    }

    public void SetBounds(RoomData room)
    {
        if (room == null)
        {
            Debug.LogError("RoomData が null です");
            return;
        }

        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;

        minXLimit = room.minXBoundary + halfWidth;
        maxXStopLimit = room.maxXBoundary - halfWidth;
        minYLimit = room.YBoundary;
        maxYLimit = room.maxYBoundary;

        currentVelocity = Vector3.zero;

        Debug.Log($"📷 カメラ境界線を設定: minX={minXLimit:F2}, maxX={maxXStopLimit:F2}, minY={minYLimit:F2}, maxY={maxYLimit:F2}");
    }

  //// --- 通常の追従処理（例） ---
  //private void LateUpdate()
  //{
  //    if (isScrolling || target == null) return;

  //    Vector3 desiredPosition = target.position;
  //    desiredPosition.z = -10f;
  //    Vector3 smoothed = Vector3.SmoothDamp(transform.position, desiredPosition, ref followVelocity, followSmoothSpeed);
  //    transform.position = smoothed;
  //}

    [SerializeField] private float smoothSpeed = 0.125f; // カメラの滑らかさ調整用

    private Vector3 currentVelocity = Vector3.zero; // SmoothDamp用

    void FixedUpdate() // 物理演算や追従はFixedUpdateの方が安定しやすい
    {
        // 演出中ではない場合のみ、通常追従を実行
        if (!isScrolling)
        {
           FollowAndClampTarget();
        }
    }

    public void ForceStopScrolling()
{
    isScrolling = false;
}

    /// <summary>
    /// ターゲットを追いかけ、指定された範囲に制限する
    /// </summary>
    private void FollowAndClampTarget()
    {
        Vector3 desiredPosition = target.transform.position;
        desiredPosition.z = -10; // Z軸を固定

        // 1. 境界制限を適用 (現在のコードのロジックを整理)
        desiredPosition = ClampCameraPosition(desiredPosition);

        // 2. スムーズな追従 (LerpやSmoothDampを使うと滑らかになる)
        // LerpよりSmoothDampの方がよりゲームカメラの追従に向いている
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position, 
            desiredPosition, 
            ref currentVelocity, 
            smoothSpeed
        );

        transform.position = smoothedPosition;
    }
    
    /// <summary>
    /// カメラの位置に境界制限を適用する
    /// </summary>
    private Vector3 ClampCameraPosition(Vector3 pos)
    {
// 1. X軸制限（minXLimit と maxXStopLimit の間だけに制限する）
    pos.x = Mathf.Clamp(pos.x, minXLimit, maxXStopLimit);
    // 2. Y軸制限（マジックナンバーを削除し、currentFixedY に完全固定する）
    // もし上下に少し動かしたいなら、minYLimit と maxYLimit を作っても良いですが、
    // 「一本道・兵站設計」なら、この 1行で十分です。
    pos.y = Mathf.Clamp(pos.y, minYLimit, maxYLimit);


        return pos;
    }
    
    // ... 演出用の変数・メソッドをここに追加 ...
    public bool isPerformingSpecialMove = false; // 演出中フラグ
    // ...


    public IEnumerator DoSpecialMove(Vector3 targetPos, float targetZoom, float duration)
{
    isPerformingSpecialMove = true; // 通常追従を停止
    
    Camera cam = Camera.main; // メインカメラを取得
    Vector3 startPosition = cam.transform.position;
    float startZoom = cam.orthographicSize;
    float timer = 0f;

    while (timer < duration)
    {
        float t = timer / duration;
        
        // Lerpで位置とズームを補間
        cam.transform.position = Vector3.Lerp(startPosition, targetPos, t);
        cam.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);

        timer += Time.deltaTime;
        yield return null;
    }

    // 最終的な値を確実に設定
    cam.transform.position = targetPos;
    cam.orthographicSize = targetZoom;

    isPerformingSpecialMove = false; // 演出終了。通常追従を再開
}

/// <summary>
    /// 指定の座標へ移動し、指定の倍率にズームする
    /// </summary>
    /// <param name="targetPos">カメラが移動する目標位置</param>
    /// <param name="targetZoom">目標のカメラサイズ (小さいほどズームイン)</param>
    public void ZoomAndMoveTo(Vector3 targetPos, float targetZoom, float delayAfterZoom)
    {
        // 他の移動演出中は実行しない
        if (isScrolling) return; 

        // ズームと移動を同時に実行するコルーチンを開始
        StartCoroutine(ZoomAndMoveCamera(targetPos, targetZoom, delayAfterZoom));
    }

    private IEnumerator ZoomAndMoveCamera(Vector3 targetPos, float targetZoom, float delayAfterZoom)
    {
        isScrolling = true; // 追従を停止
        
        Vector3 startPos = transform.position;
        float startZoom = Camera.main.orthographicSize;
        float t = 0f;

        while (t < scrollDuration)
        {
            float progress = t / scrollDuration;
            
            // 1. 位置の移動
            transform.position = Vector3.Lerp(startPos, targetPos, progress);

            // 2. ズームの変更 (カメラサイズを滑らかに変更)
            Camera.main.orthographicSize = Mathf.Lerp(startZoom, targetZoom, progress);
            
            t += Time.deltaTime;
            yield return null;
        }




        // 最終的な値を確定
        transform.position = targetPos;
        Camera.main.orthographicSize = targetZoom;
        // ★ ズームアニメーション完了後、ここで指定された時間だけ追従停止状態を維持する
    if (delayAfterZoom > 0)
    {
        yield return new WaitForSeconds(delayAfterZoom);
    }


        // 慣性のリセットとフラグ解除（通常追従の再開）
        currentVelocity = Vector3.zero;
        isScrolling = false;

        // 必要に応じて、ズーム後の maxXStopLimit などの更新処理を追加
        // (ここでは省略し、ズーム後の追従は targetPos の周囲で行われます)
    }

    /// <summary>
    /// カメラを元のズーム率に戻す
    /// </summary>
    public void ResetZoom()
    {
        // 演出中ではない場合のみ、元のズームに戻すコルーチンを実行
        if (!isScrolling && Camera.main.orthographicSize != defaultZoom)
        {
            StartCoroutine(ZoomAndMoveCamera(transform.position, defaultZoom,delayAfterZoom));
        }
    }

}