using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("基本設定")]
    public int hp = 3;
    public float moveSpeed = 2f;
    public float mass = 1f;
    public string roomID = "";

    [Header("ダメージ設定")]
    public float damageAmount = 3f;

    [Header("移動設定")]
    public float moveRange = 5f;
    public bool facePlayer = true;

    [Header("地面判定")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    [Header("壁判定")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;

    [Header("方向転換設定")]
    public float turnCooldown = 0.5f;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Vector3 startPosition;
    protected int moveDirection = 1;
    protected Transform player;
    protected bool isGrounded;
    protected bool hitWall;
    public LayerMask StageLayer;

    protected Animator anim;
    protected float lastTurnTime = -1f;

    public EnemyState currentState = EnemyState.Idle;
    protected float alertTimer = 0f;
    protected bool alertReactionTriggered = false;

    public static System.Action<GameObject> OnAnyEnemyDeath;

    public enum EnemyState
    {
        Idle,
        Alert,
        Chase
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // roomID が未設定なら、座標からRoom を自動判定
        if (string.IsNullOrEmpty(roomID))
        {
            RoomManager roomManager = FindObjectOfType<RoomManager>();
            if (roomManager != null)
            {
                RoomData room = roomManager.GetRoomByPosition(transform.position);
                if (room != null)
                {
                    roomID = "Room" + room.roomID;
                    Debug.Log($"敵 {gameObject.name} の roomID を自動設定: {roomID}");
                }
            }
        }
    }

    protected virtual void Update()
    {
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
        }
    }

    protected virtual void Move()
    {
        bool shouldTurn = false;

        if (hitWall)
        {
            shouldTurn = true;
            hitWall = false;
            TurnAround();
        }

        if (!GroundChk())
        {
            shouldTurn = true;
        }

        if (shouldTurn && CanTurn())
        {
            TurnAround();
        }

        rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
    }

    protected bool CanTurn()
    {
        return Time.time - lastTurnTime > turnCooldown;
    }

    protected void TurnAround()
    {
        moveDirection *= -1;
        lastTurnTime = Time.time;

        if (moveDirection > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    protected virtual void HandleIdle()
    {
        if (wallCheck != null)
            CheckWall();
        Move();

        if (player != null && Vector3.Distance(transform.position, player.position) < moveRange)
        {
            currentState = EnemyState.Alert;
            alertTimer = 1.0f;
            alertReactionTriggered = false;
            Debug.Log("見つけた！");
        }
    }

    protected virtual void HandleAlert()
    {
        if (!alertReactionTriggered)
        {
            alertReactionTriggered = true;
            StartCoroutine(AlertReaction());
        }

        alertTimer -= Time.deltaTime;

        if (alertTimer <= 0)
        {
            currentState = EnemyState.Chase;
            alertReactionTriggered = false;
            Debug.Log("襲いかかる！");
        }
    }

    protected virtual void HandleChase()
    {
        // 派生先でオーバーライド
    }

    protected void CheckWall()
    {
        Vector2 wallCheckPos = wallCheck.position;
        Vector2 rayDirection = Vector2.right * moveDirection;
        hitWall = Physics2D.Raycast(wallCheckPos, rayDirection, wallCheckDistance, wallLayer);
        Debug.DrawRay(wallCheckPos, rayDirection * wallCheckDistance, Color.blue, 0.1f);
    }

    protected bool GroundChk()
    {
        Vector3 scale = transform.localScale;
        Vector3 startposition = transform.position + transform.right * 0.3f * scale.x;
        Vector3 endposition = startposition - transform.up * 0.55f;

        Debug.DrawLine(startposition, endposition, Color.red);

        return Physics2D.Linecast(startposition, endposition, StageLayer);
    }

    protected void FacePlayer()
    {
        if (player == null) return;

        bool shouldFlip = player.position.x < transform.position.x;
        spriteRenderer.flipX = shouldFlip;
    }

    public virtual void TakeDamage(int damageAmount, Vector3 bulletPos = default)
    {
        hp -= damageAmount;

        // ノックバック処理
        if (bulletPos != default && rb != null)
        {
            Vector2 knockbackDir = (transform.position - bulletPos).normalized;
            rb.AddForce(knockbackDir * (5f / mass), ForceMode2D.Impulse);
        }

        StartCoroutine(FlashEffect());

        if (hp <= 0)
        {
            Die();
        }
    }

    protected IEnumerator FlashEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    protected IEnumerator AlertReaction()
    {
        Vector3 originalPos = transform.position;
        float jumpHeight = 0.5f;
        float jumpDuration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float jumpPercent = elapsedTime / (jumpDuration / 2);
            transform.position = originalPos + Vector3.up * jumpHeight * jumpPercent;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < jumpDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float jumpPercent = elapsedTime / (jumpDuration / 2);
            transform.position = originalPos + Vector3.up * jumpHeight * (1 - jumpPercent);
            yield return null;
        }

        transform.position = originalPos;

        GameObject exclamationMark = new GameObject("ExclamationMark");
        exclamationMark.transform.SetParent(transform);
        exclamationMark.transform.localPosition = new Vector3(0, 1f, 0);

        TextMesh textMesh = exclamationMark.AddComponent<TextMesh>();
        textMesh.text = "！";
        textMesh.fontSize = 10;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        Destroy(exclamationMark);
    }

    protected virtual void Die()
    {
        Gmanager.Instance.Enemydefeatfun();
        OnAnyEnemyDeath?.Invoke(this.gameObject);
        Debug.Log("イベントを飛ばします");

        // 敵撃破フラグを立てる
        if (!string.IsNullOrEmpty(roomID))
        {
            Gmanager.Instance.SetFlag(roomID + "_Enemy_Defeated", true);
            Debug.Log($"フラグ '{roomID}_Enemy_Defeated' を立てました");
        }

        Destroy(gameObject);
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {
            bullet bullet = other.GetComponent<bullet>();
            if (bullet != null)
            {
                TakeDamage(3);
            }
        }
    }
}
