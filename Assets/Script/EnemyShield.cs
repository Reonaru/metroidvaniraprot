using System.Collections;
using UnityEngine;

/// <summary>
/// 盾役敵クラス - 複数敵パターンの戦略性を支える
///
/// === 盾ロジック設計書 ===
/// 防御範囲: 敵の移動方向（前方180度）
/// 盾耐久度: 3回のプレイヤー弾で破壊
/// 破壊時状態: 盾ビジュアルが非表示→通常敵モード（防御無効）
/// ビジュアル: shieldObject の GameObject を非表示化（色変化ではなく disable）
///
/// === 動作フロー ===
/// 1. プレイヤー弾がこの敵に衝突 → OnTriggerEnter2D で検出
/// 2. 盾が有効か判定（shieldHP > 0 && 防御範囲内）
/// 3. 盾が有効 → 弾を消去、敵には効果なし、盾HP減少
/// 4. 盾無効 → 通常のダメージ処理（敵HP減少）
/// 5. 盾HP が 0 → シールドを破壊（disable）、その後は通常敵化
/// </summary>
public class EnemyShield : EnemyBase
{
    [Header("盾設定")]
    public int shieldHP = 3;  // 盾の耐久度（プレイヤー弾3回で破壊）
    public GameObject shieldObject;  // 盾の可視化オブジェクト（検査官で指定）

    private int currentShieldHP;
    private bool shieldActive = true;

    protected override void Start()
    {
        base.Start();
        currentShieldHP = shieldHP;
    }

    protected override void HandleChase()
    {
        float step = moveSpeed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > moveRange)
        {
            currentState = EnemyState.Idle;
            Debug.Log("見失った…");
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {
            if (shieldActive && IsInShieldRange(other.transform.position))
            {
                // 盾が有効 → 弾を防ぐ
                HandleShieldBlock(other);
                return;  // 通常のダメージ処理を実行しない
            }

            // 盾が無効 or 範囲外 → 通常ダメージ処理
            TakeDamage((int)damageAmount, other.transform.position);
        }
    }

    private bool IsInShieldRange(Vector3 bulletPos)
    {
        Vector2 direction = (bulletPos - transform.position).normalized;
        float angle = Vector2.Angle(Vector2.right * moveDirection, direction);
        return angle <= 90f;  // 前方180度内（左右各90度）
    }

    private void HandleShieldBlock(Collider2D bulletCollider)
    {
        // 盾HP減少
        currentShieldHP--;
        Debug.Log($"【盾ダメージ】{gameObject.name}: 盾HP {currentShieldHP + 1}/{shieldHP} → {currentShieldHP}/{shieldHP}");

        // 弾を消去
        Destroy(bulletCollider.gameObject);

        // 盾が破壊された
        if (currentShieldHP <= 0)
        {
            BreakShield();
        }
    }

    private void BreakShield()
    {
        shieldActive = false;
        Debug.Log($"【盾破壊】{gameObject.name}: 盾が破壊されました。敵は通常モードに切り替わります。");

        // 盾ビジュアルを非表示
        if (shieldObject != null)
        {
            shieldObject.SetActive(false);
            Debug.Log($"  - 盾ビジュアル '{shieldObject.name}' を非表示化");
        }
        else
        {
            Debug.LogWarning($"  - 警告: shieldObject が未設定です（Inspector で指定してください）");
        }

        // 盾破壊時のビジュアルフィードバック（敵本体の色が変わる等）
        StartCoroutine(ShieldBreakEffect());
    }

    private IEnumerator ShieldBreakEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    private void OnDrawGizmosSelected()
    {
        if (!enabled) return;

        // 盾が有効な場合のみ防御範囲を描画
        if (shieldActive)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        // 敵の前方180度の範囲を描画
        int segments = 18;  // 18セグメント = 前方180度
        Vector3 center = transform.position;
        float radius = 2f;

        Vector2 direction = Vector2.right * moveDirection;
        for (int i = 0; i <= segments; i++)
        {
            float angle = -90f + (180f / segments) * i;
            Vector3 pos1 = center + (Quaternion.AngleAxis(angle, Vector3.forward) * direction) * radius;

            if (i > 0)
            {
                float prevAngle = -90f + (180f / segments) * (i - 1);
                Vector3 pos0 = center + (Quaternion.AngleAxis(prevAngle, Vector3.forward) * direction) * radius;
                Gizmos.DrawLine(pos0, pos1);
            }
        }

        // 盾HP をインスペクタに表示
        Gizmos.color = Color.white;
        Gizmos.DrawLine(center + Vector3.up * 1.5f, center + Vector3.up * 1.5f + Vector3.right * 0.1f);
    }
}
