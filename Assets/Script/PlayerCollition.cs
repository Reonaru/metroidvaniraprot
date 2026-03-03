using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class PlayerCollition : MonoBehaviour
{

	[Header("プレイヤー設定")]
	public float health = 100f;
	public float invincibilityTime = 1f;

    [Header("削除するオブジェクト設定")]
    [SerializeField] private GameObject[] objectsToRemove;  // 削除するオブジェクトの配列

    [Header("デバッグ")]
    [SerializeField] private bool showDebugLog = true;

	private bool isInvincible = false;
	private Playersc playerScript;
	private SpriteRenderer playerRenderer;
    private CameraManager1 cameramanager1;

	private ItemCounter itemCounter;

	public bool isground = false;
	public GameObject rightbox;
	public GameObject leftbox;
	private bool canMoveRight = true;

    [Header("ダメージ設定")]
    public float damageAmount = 10f;  // 受けるダメージ量
    public float blinkDuration = 0.1f;  // 点滅の間隔

    private Coroutine blinkCoroutine;  // 点滅用のコルーチン


    void Start()
    {
        // 親のPlayerscスクリプトを取得
        playerScript = GetComponentInParent<Playersc>();
        playerRenderer = GetComponentInParent<SpriteRenderer>();
        itemCounter = FindObjectOfType<ItemCounter>();  
        cameramanager1 = FindObjectOfType<CameraManager1>();

        // 当たり判定用のColliderを設定
        SetupCollisionDetector();
    }


 
     void SetupCollisionDetector()
    {
        // このオブジェクトにCircleCollider2Dを追加（ダメージ判定用）
        CircleCollider2D damageCollider = gameObject.AddComponent<CircleCollider2D>();
        damageCollider.radius = 0.6f;
        damageCollider.isTrigger = true; // Triggerとして設定
        
        Debug.Log("当たり判定システム初期化完了");
    }

    // Trigger判定
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"何かに触れました: {other.name}");
        

    if (other.CompareTag("enemy"))
    {
        TakeDamage(damageAmount);
        Debug.Log("敵にぶつかりました！");
    }

        
        // アイテムとの接触
        if (other.CompareTag("Item"))
        {
            CollectItem(other.gameObject);
			
        }

//		if (other.CompareTag("ground")){
//			ground();
//		}

		if (other.CompareTag("wall")){
			deletewall(other.gameObject);
			
		}
        

    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ground"))
        {
            ground_false();
        }
    }


	public bool CheckRightBoxCollision()
    {


        if (rightbox == null) return true;

        // rightboxのColliderを取得
        Collider2D rightboxCollider = rightbox.GetComponent<Collider2D>();
        if (rightboxCollider == null) return true;

        // rightboxの範囲内でgroundタグのオブジェクトを検索
        Collider2D groundCollider = Physics2D.OverlapBox(
            rightboxCollider.bounds.center,
            rightboxCollider.bounds.size,
            0f,
            LayerMask.GetMask("ground") // groundがあるレイヤーを指定
        );
//        Debug.Log($"何かと重なっているか: {groundCollider}");
//		        Debug.Log($"   - タグ: '{groundCollider.tag}'");
//	        Debug.Log($"   - 名前: {groundCollider.name}");
        // groundタグのオブジェクトが見つかった場合
        if (groundCollider != null && groundCollider.CompareTag("ground"))
        {
            ground_right();
            canMoveRight = false;
            Debug.Log("collitionのrightの当たり判定");
			return false;
        }
		else 
		{
			canMoveRight = true;
			return true;
		}
    }



 void ground(){
	isground = true;
 }

 void ground_false(){
    isground = false;
 }

 void ground_right(){
    
	            Debug.Log("行けたみたい？");
 }

 void CollectItem(GameObject item)
    {
        Debug.Log($"アイテム取得: {item.name}");
        
        // アイテムの種類に応じた処理
        if (item.name.Contains("Coin"))
        {
            // コイン処理
            Debug.Log("コインを取得しました！");
			
        }
        itemCounter.AddItem();
        Destroy(item);
		RemoveTargetObjects();

    }

  void deletewall(GameObject wall)
    {
		
        Debug.Log($"壊せる: {wall.name}");
        

        Destroy(wall);


    }


    private void RemoveTargetObjects()
    {
        // 配列で指定されたオブジェクトを削除
        RemoveObjectAtIndex(1);
    }

    private void RemoveObjectAtIndex(int index)
    {
        if (objectsToRemove == null) return;
        
        GameObject obj = objectsToRemove[index];
		if (obj != null)
		{
			Destroy(obj);
		}

    }

// ダメージを受ける処理
void TakeDamage(float damage)
{
    if (isInvincible) return;  // 無敵時間中は何もしない
    
    // HPを減らす
    health -= damage;
    Debug.Log($"ダメージを受けました！ 残りHP: {health}");
    
    // HPが0以下になったら
    if (health <= 0)
    {
        health = 0;
        GameOver();
        return;
    }
    
    // 無敵状態にして点滅開始
    StartInvincibility();
}

// 無敵状態と点滅の開始
void StartInvincibility()
{
    isInvincible = true;
    
    // 点滅処理を開始
    if (blinkCoroutine != null)
    {
        StopCoroutine(blinkCoroutine);
    }
    blinkCoroutine = StartCoroutine(BlinkEffect());
    
    // 無敵時間終了
    Invoke("EndInvincibility", invincibilityTime);
}

// 点滅エフェクト
IEnumerator BlinkEffect()
{
    while (isInvincible)
    {
        // 透明にする
        playerRenderer.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(blinkDuration);
        
        // 元に戻す
        playerRenderer.color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(blinkDuration);
    }
}

// 無敵状態終了
void EndInvincibility()
{
    isInvincible = false;
    
    // 点滅を停止して元の色に戻す
    if (blinkCoroutine != null)
    {
        StopCoroutine(blinkCoroutine);
        blinkCoroutine = null;
    }
    playerRenderer.color = new Color(1, 1, 1, 1f);
    
    Debug.Log("無敵時間終了");
}

// ゲームオーバー処理
void GameOver()
{
    Debug.Log("ゲームオーバー！");
    // ここにゲームオーバー時の処理を追加
    // 例：playerScript.enabled = false;
    playerScript.enabled = false;
    playerRenderer.color = new Color(1, 1, 1, 0.1f);
    playerScript.ResetPosition();
    cameramanager1.ResetCameraForRetry();
}

}