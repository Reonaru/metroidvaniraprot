using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("基本設定")]
    public int hp = 3;
    public float moveSpeed = 2f;
    public int damage = 1;
    
    [Header("移動設定")]
    public float moveRange = 5f; // 移動範囲
    public bool facePlayer = true; // プレイヤーの方を向くか
    
    [Header("地面判定")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    
    [Header("壁判定")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    
    [Header("方向転換設定")]
    public float turnCooldown = 0.5f; // 方向転換のクールダウン時間
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private int moveDirection = 1; // 1=右, -1=左
    private Transform player;
    private bool isGrounded;
    private bool hitWall;
    public LayerMask StageLayer;
    
    // 連続方向転換を防ぐための変数
    private float lastTurnTime = -1f;

    public EnemyState currentState = EnemyState.Idle; // 現在の状態
    private float alertTimer = 0f;                   // 警戒時間の計測用
    private int dashDirection;

    public static System.Action<GameObject> OnAnyEnemyDeath;
    
    public enum EnemyState
    {
        Idle,      // 待機時間
        Alert,     // 見つけた時間（ビビってる時間）
        Chase,      // 襲う時間
        Charge,
        Attack,
        Cooldown
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        
        // プレイヤーを探す
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }
    
    void Update()
    {
      //  CheckWall();
      //  Move();
        
       // if (facePlayer && player != null)
       //     FacePlayer();

    // 現在の状態に応じて、実行する関数を切り替える
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;
            case EnemyState.Alert:
                HandleAlert();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.Charge:
                HandleCharge();
                break;
            case EnemyState.Attack:
                HandleAttack();
                break;
            case EnemyState.Cooldown:
                alertTimer -= Time.deltaTime;
                spriteRenderer.color = Color.white; 
                if (alertTimer <= 0) currentState = EnemyState.Idle;
                break;
        }

    }
    
    void Move()
    {
        bool shouldTurn = false;
        
        // 壁にぶつかったら方向転換フラグを立てる
        if (hitWall)
        {
            shouldTurn = true;
            hitWall = false;
                        TurnAround();
        }
        
        // 地面がなければ方向転換フラグを立てる
        if (!GroundChk())
        {
            shouldTurn = true;
        }
        
        // 方向転換が必要で、クールダウン時間が経過していたら方向転換
        if (shouldTurn && CanTurn())
        {
            TurnAround();
        }
        
        // 移動
        rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
    }
    
    // 方向転換可能かチェック
    bool CanTurn()
    {
        return Time.time - lastTurnTime > turnCooldown;
    }
    
    // 方向転換実行
    void TurnAround()
    {
        moveDirection *= -1;
        lastTurnTime = Time.time;

// moveDirectionがプラスなら右、マイナスなら左に反転
    // (元々の画像が右向きの場合)
    if (moveDirection > 0) {
        spriteRenderer.flipX = true;
    } else {
        spriteRenderer.flipX = false;
    }

    }
    // groundへの接地判定用だが不要な認識　※使っているかどうか確認
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    
void HandleIdle()
{
// 監視ロジック：プレイヤーとの距離が5以下なら
if (Vector3.Distance(transform.position, player.position) < 10f)
{
        currentState = EnemyState.Alert; // モード変更！
        alertTimer = 1.0f;              // 1秒間ビビらせる予約
        Debug.Log("見つけた！");
}
}

void HandleAlert()
{
    alertTimer -= Time.deltaTime; // カウントダウン
    
    // 1秒経ったら襲う
    if (alertTimer <= 0)
    {
        currentState = EnemyState.Chase; // モード変更！
        Debug.Log("襲いかかる！");
    }
}

void HandleChase()
{
    float step = moveSpeed * Time.deltaTime;

    float distance = Vector3.Distance(transform.position, player.position);

    if (distance < 3.0f) 
    {
        currentState = EnemyState.Charge; // 🚩 ここでStateを切り替える！
        alertTimer = 1.0f;               // 溜める時間をセット（1秒間など）
        rb.velocity = Vector2.zero;      // 溜め中は足を止める
        Debug.Log("射程圏内！溜め状態（Charge）へ移行します");
        return; // このフレームの追跡処理はここで終了
    }

    // 距離が15以上離れたら、諦めて待機状態に戻る
    if (distance > 10.0f)
    {
        currentState = EnemyState.Idle;
        Debug.Log("見失った…");
        return; // これ以上下の移動処理はさせない
    }

    // プレイヤーに向かって移動するロジックをここに書く
    transform.position = Vector3.MoveTowards(transform.position, player.position, step);
}

void HandleCharge()
{
    rb.velocity = Vector2.zero; // その場で止まる
    spriteRenderer.color = Color.yellow; // 「来るぞ！」と色で警告
    alertTimer -= Time.deltaTime;

    if (alertTimer <= 0)
    {
        // 突進方向を確定させてから突進状態へ
        dashDirection = (player.position.x > transform.position.x) ? 1 : -1;
        currentState = EnemyState.Attack;
        alertTimer = 0.5f; // 突進する時間
    }
}

void HandleAttack()
{
    // 猛スピードで突進！
    rb.velocity = new Vector2(dashDirection * moveSpeed * 3f, rb.velocity.y);
    alertTimer -= Time.deltaTime;

    if (alertTimer <= 0)
    {
        currentState = EnemyState.Cooldown;
        alertTimer = 1.0f; // 攻撃後の大きな隙
    }
}

    //壁への接触判定
    void CheckWall()
    {
        Vector2 wallCheckPos = wallCheck.position;
        Vector2 rayDirection = Vector2.right * moveDirection; 
        hitWall = Physics2D.Raycast(wallCheckPos, rayDirection, wallCheckDistance, wallLayer);
        Debug.DrawRay(wallCheckPos, rayDirection * wallCheckDistance, Color.blue, 0.1f);
    }
    

    // groundへの接地判定　※使っているかどうか確認
    bool GroundChk()
    {
        // transform.localScaleの正負によってEnemyをx方向に反転する
        Vector3 scale = transform.localScale;
        // 始点が常にEnemyの進行方向に出るようにstartpositionを決める
        Vector3 startposition = transform.position + transform.right * 0.3f * scale.x;
        // startpostionから足元までを終点とする
        Vector3 endposition = startposition - transform.up * 0.55f;

        // Debug用に始点と終点を表示する
        Debug.DrawLine(startposition, endposition, Color.red);

        bool testlayer = Physics2D.Linecast(startposition, endposition, StageLayer);


        // Physics2D.Linecastを使い、ベクトルとStageLayerが接触していたらTrueを返す
        return Physics2D.Linecast(startposition, endposition, StageLayer);
    }

    // プレイヤーに反応するかどうか
    void FacePlayer()
    {
        if (player == null) return;
        
        bool shouldFlip = player.position.x < transform.position.x;
        spriteRenderer.flipX = shouldFlip;
    }
    
    // ダメージを受ける
    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
        
        // 点滅エフェクト（簡易版）
        StartCoroutine(FlashEffect());
        
        if (hp <= 0)
        {
            Die();
        }
    }
    
    System.Collections.IEnumerator FlashEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
    
    void Die()
    {


        // 死亡時の処理
        Gmanager.Instance.Enemydefeatfun();
        OnAnyEnemyDeath?.Invoke(this.gameObject);
                            Debug.Log("イベントを飛ばします");

        Destroy(gameObject);
    }
    
    // プレイヤーとの接触判定
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {
            // プレイヤーにダメージを与える
            bullet bullet = other.GetComponent<bullet>();
            if (bullet != null)
            {
                TakeDamage(3);
            }
        }
    }
}