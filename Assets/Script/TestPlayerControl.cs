using UnityEngine;

/// <summary>
/// テストシーン用簡易プレイヤー制御スクリプト
///
/// === 操作方法 ===
/// 移動: A/D キー または 左右矢印キー
/// ジャンプ: Space キー
/// 発射: X キー（毎回発射、連射可能）
///
/// === 機能 ===
/// - 基本的な2D移動とジャンプ
/// - 弾発射（bullet プレハブが必要）
/// - シンプルな操作入力（テスト用）
/// </summary>
public class TestPlayerControl : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("射撃設定")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 10f;

    [Header("地面判定")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private int bulletCount = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // shootPoint が未設定の場合は、自動で作成
        if (shootPoint == null)
        {
            GameObject shootPointObj = new GameObject("ShootPoint");
            shootPointObj.transform.SetParent(transform);
            shootPointObj.transform.localPosition = new Vector3(0.5f, 0, 0);
            shootPoint = shootPointObj.transform;
        }

        // bulletPrefab が未設定の場合は警告
        if (bulletPrefab == null)
        {
            Debug.LogWarning("TestPlayerControl: bulletPrefab が未設定です。Inspector で指定してください。");
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleShoot();
        CheckGround();
    }

    private void HandleMovement()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveInput = 1f;

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // スプライト反転（右向き/左向き）
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (moveInput > 0)
                spriteRenderer.flipX = false;
            else if (moveInput < 0)
                spriteRenderer.flipX = true;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("ジャンプ");
        }
    }

    private void HandleShoot()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("bulletPrefab が設定されていません");
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            // プレイヤーの向きに合わせて弾速度を設定
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            float direction = spriteRenderer != null && spriteRenderer.flipX ? -1f : 1f;
            bulletRb.velocity = new Vector2(bulletSpeed * direction, 0);
        }

        bulletCount++;
        Debug.Log($"【発射】第 {bulletCount} 回目の弾を発射");
    }

    private void CheckGround()
    {
        Vector3 startPos = transform.position + Vector3.down * 0.5f;
        Vector3 endPos = startPos + Vector3.down * groundCheckDistance;

        Debug.DrawLine(startPos, endPos, Color.green);

        isGrounded = Physics2D.Linecast(startPos, endPos, groundLayer);
    }
}
