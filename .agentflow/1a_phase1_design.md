# Phase 1: EnemyShield 仕様確定

## 目的
EnemyShield の盾ロジックを確定させ、実装前に全パラメータを決定する。

---

## 盾システム仕様書（最終版）

### 1. 基本設定
| パラメータ | 値 | 根拠 |
|-----------|-----|------|
| **盾耐久度** | 3 回 | 2〜5秒での破壊を想定（敵速度・弾速度による） |
| **防御範囲** | 前方 180 度 | 敵の移動方向を基準に左右各 90 度 |
| **敵追従速度** | 2 m/s | 基本敵と同等のバランス |
| **敵 HP** | 3 | 盾破壊後のゲームバランス調整用 |

### 2. 防御範囲の詳細設計
```
敵が右向きの場合:
  moveDirection = 1
  防御範囲: 右 0° ± 90° = -90°〜+90° (前方半円)

敵が左向きの場合:
  moveDirection = -1
  防御範囲: 左 180° ± 90° = 90°〜270° (前方半円)

判定方法: Vector2.Angle(Vector2.right * moveDirection, 弾方向) <= 90f
```

### 3. 盾破壊時の状態遷移
```
盾 HP: 3
↓ 弾衝突 1 回目 → 盾 HP: 2
↓ 弾衝突 2 回目 → 盾 HP: 1
↓ 弾衝突 3 回目 → 盾 HP: 0
↓
[盾破壊]
- shieldObject を disable（非表示化）
- shieldActive = false（防御無効化）
- 敵本体が 0.2 秒黄色フラッシュ
- 以降すべての弾が通常ダメージ処理
```

### 4. ビジュアル・オーディオフィードバック

| イベント | 実装内容 | 予定 |
|---------|--------|------|
| **盾衝突時** | Debug.Log 出力（敵 HP 表示） | ✓ 実装済み |
| **盾破壊時** | 敵本体が 0.2 秒黄色フラッシュ | ✓ 実装済み |
| **盾破壊時** | shieldObject.SetActive(false) | ✓ 実装済み |
| **防御範囲可視化** | Gizmo で前方 180 度を描画（テスト用） | ✓ 実装済み |

---

## EnemyShield.cs の実装確認

### 既実装機能

#### ✓ OnTriggerEnter2D（盾判定）
```csharp
protected override void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("bullet"))
    {
        if (shieldActive && IsInShieldRange(other.transform.position))
        {
            HandleShieldBlock(other);  // 盾で防ぐ
            return;
        }
        TakeDamage((int)damageAmount, other.transform.position);  // 通常ダメージ
    }
}
```

#### ✓ IsInShieldRange（前方 180 度判定）
```csharp
private bool IsInShieldRange(Vector3 bulletPos)
{
    Vector2 direction = (bulletPos - transform.position).normalized;
    float angle = Vector2.Angle(Vector2.right * moveDirection, direction);
    return angle <= 90f;
}
```

#### ✓ HandleShieldBlock（盾 HP 減少と弾破壊）
```csharp
private void HandleShieldBlock(Collider2D bulletCollider)
{
    currentShieldHP--;
    Debug.Log($"【盾ダメージ】{gameObject.name}: 盾HP {currentShieldHP + 1}/{shieldHP} → {currentShieldHP}/{shieldHP}");
    Destroy(bulletCollider.gameObject);
    if (currentShieldHP <= 0) BreakShield();
}
```

#### ✓ BreakShield（盾破壊処理）
```csharp
private void BreakShield()
{
    shieldActive = false;
    Debug.Log($"【盾破壊】{gameObject.name}: 盾が破壊されました。");
    if (shieldObject != null)
        shieldObject.SetActive(false);
    StartCoroutine(ShieldBreakEffect());
}
```

#### ✓ ShieldBreakEffect（視覚フィードバック）
```csharp
private IEnumerator ShieldBreakEffect()
{
    Color originalColor = spriteRenderer.color;
    spriteRenderer.color = Color.yellow;
    yield return new WaitForSeconds(0.2f);
    spriteRenderer.color = originalColor;
}
```

#### ✓ OnDrawGizmosSelected（防御範囲表示）
盾が有効な場合は緑色、無効な場合は赤色で前方 180 度を描画。

---

## Phase 1 検証チェックリスト

- [x] 盾耐久度を 3 に設定（EnemyShield.cs で確認）
- [x] 防御範囲を前方 180 度に設定（IsInShieldRange で確認）
- [x] 盾破壊時の状態遷移を実装（BreakShield で確認）
- [x] ビジュアルフィードバックを実装（ShieldBreakEffect で確認）
- [x] Gizmo 可視化を実装（OnDrawGizmosSelected で確認）
- [x] shieldObject 外部参照を設計（Inspector 設定用）

---

## 依存する外部実装

### 必須：bullet クラス
- bullet タグで検出（EnemyBase.OnTriggerEnter2D）
- GetComponent<bullet>() で取得
- ※ EnemyShield では bullet オブジェクト自体を破壊（Destroy）するため、bullet クラスの詳細は依存しない

### 必須：プレイヤー弾発射機構
- Player がアイテムを持って弾を発射する仕組みが必要
- 弾は bullet タグで Collider2D を持つ必要がある

### オプション：PlayerCollision.cs
- 存在しない（使用していない）
- 弾の衝突処理は EnemyBase.OnTriggerEnter2D で実装

---

## Phase 1 完了条件

- ✓ 盾ロジック設計書が EnemyShield.cs のコメント頭に記載
- ✓ すべてのパラメータが数値で決定された
- ✓ 防御範囲・耐久度・状態遷移が実装コードで確認可能
- ✓ テストシーン構築スクリプト（CreateTestShieldScene.cs）が準備完了

---

## 次フェーズへの移行

Phase 2（実装）は既に完了している。
→ Phase 3（テスト実行）へ直接進む。

ただし、以下を確認してからテスト開始：
1. Assets/Scenes/TestShield.unity の作成実行
2. プレイヤー弾発射機構の確認
3. デバッグモード有効化（Gizmo 表示）

