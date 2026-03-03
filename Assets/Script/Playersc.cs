using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
/// <summary>
/// アクター操作・制御クラス
/// </summary>
public class Playersc : MonoBehaviour
{
	// 変数宣言部
 	Rigidbody2D rbody2D;
    private bool isjumped = false;
    public PlayerCollition coli;
    public CameraManager1 cam;


    public float maxSpeed = 5f;    
    public float airDeceleration = 5f;  
    public float deceleration = 15f;  
    public float acceleration = 20f;   

    private float horizontalInput;
	private float jumpBufferCounter;
    public bool jumpInput;
    private bool jumpInputReleased;

    [Header("コヨーテタイム設定")]
    public float coyoteTime = 0.1f;       // 地面を離れてもジャンプできる時間
    public float jumpBufferTime = 0.1f;

    [Header("弾設定")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    [Header("スタートの場所")]
    private Vector3 startPosition;

    // Start（オブジェクト有効化時に1度実行）
    void Start()
    {

        startPosition = transform.position;
        rbody2D = GetComponent<Rigidbody2D>(); 

		if (coli == null)
        {
            coli = FindObjectOfType<PlayerCollition>();
            
            // 見つからなかった場合の警告
            if (coli == null)
            {
                Debug.LogWarning("PlayerCollitionオブジェクトが見つかりませんでした！");
            }
        // 重要な設定
        rbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;  // 連続衝突検出
        rbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;                   // スリープ無効
        rbody2D.freezeRotation = true;                                         // 回転を固定
        
        // Physics Material 2Dの設定
        PhysicsMaterial2D playerMaterial = new PhysicsMaterial2D("PlayerMaterial");
        playerMaterial.friction = 0f;        // 摩擦を0にして滑らないように
        playerMaterial.bounciness = 0f;      // 反発を0に


        }

    }
 
    // Update（1フレームごとに1度ずつ実行）
    void Update()
    {

        // 水平入力
       horizontalInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow))
		{
        horizontalInput = 1f;
        transform.localScale = new Vector3(1, 1, 1);
	//	bool canMoveRight = coli.CheckRightBoxCollision();
	////	if (!canMoveRight)
	//	{
	//		horizontalInput = 0;
	//		Debug.Log("ブロック");
	//	}
		}
            
        else if (Input.GetKey(KeyCode.LeftArrow)){
            horizontalInput = -1f;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // ジャンプ入力
        if (Input.GetKeyDown(KeyCode.Space))
        {
		if (jumpInput == false && coli.isground == true){
            jumpInput = true;
            jumpBufferCounter = jumpBufferTime;
		//	coli.isground = false;
			rbody2D.AddForce(Vector2.up * 300);
			}
			Debug.Log($"coli.isground:{coli.isground}");
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpInputReleased = true;
        }

        if (Input.GetKey(KeyCode.A) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKey(KeyCode.B))
        {
            Gmanager.Instance.SetCurrentState(GameState.Prepare);
        }


        if (Input.GetKey(KeyCode.C))
        {
                Debug.Log($"C cameraPos:{Camera.main.transform.position.x}");
        }





	
//	if (Input.GetKeyDown("space"))
//	{
//		if (isjumped == false && coli.isground == true){
//	rbody2D.AddForce(Vector2.up * 300);
//	FlagManager.Instance.SetFlag("ジャンプ");
//	isjumped = true;
//	coli.isground = false;
//		}
//	}

    HandleMovement();
		if (coli.isground == true && jumpInput == true)
		{
			jumpInput = false;
		}
	//	if (FlagManager.Instance.IsFlagOn("ジャンプ"))
     //   {
     //   Debug.Log("ジャンプした！");
      //  }




    }

    // ★ このメソッドはプレイヤーが持っている
    public void ResetPosition()
    {
        transform.position = startPosition;

        // Rigidbody2Dのリセット処理など...
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

        void Fire()
        {
            if (bulletPrefab == null || firePoint == null) return;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Vector2 fireDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            bullet.GetComponent<bullet>().SetDirection(fireDirection);
        }




    void HandleMovement()
    {
        float targetSpeed = horizontalInput * maxSpeed;
        float currentSpeed = rbody2D.velocity.x;
        
        // 加速・減速の計算
        float speedDifference = targetSpeed - currentSpeed;
        
        // 加速度の決定（地上か空中かで変わる）
        float accelRate;
        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            // 加速中
            accelRate = acceleration;
        }
        else
        {
            // 減速中（地上か空中かで減速度が違う）
            accelRate = (coli != null && coli.isground) ? deceleration : airDeceleration;
        }
        
        // 力を計算して適用
        float movement = speedDifference * accelRate * Time.deltaTime;
        
        // 新しい速度を設定（Y速度は保持）
        rbody2D.velocity = new Vector2(
            Mathf.Clamp(currentSpeed + movement, -maxSpeed, maxSpeed),
            rbody2D.velocity.y
        );
    }

}