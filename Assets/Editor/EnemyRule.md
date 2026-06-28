# 敵生成ルール

## 📋 概要

敵は `EnemyBase` を継承して、各敵タイプが `HandleChase()` をオーバーライドして追跡挙動を定義。
EditorScript で敵オブジェクトを自動生成。

---

## 🎯 敵タイプ一覧

### Enemy2D（基本敵）
| 属性 | 値 |
|------|-----|
| HP | 3 |
| 移動速度 | 2 |
| 検知範囲 | 5 |
| ダメージ | 3 |
| 重さ（mass） | 0.5 |
| 特徴 | 軽い、標準的な追跡 |

**行動パターン：**
- プレイヤーを検知 → Alert 状態（1秒ジャンプ + びっくりマーク）
- Chase 状態で直線追跡
- 5 ユニット以上離れると Idle に戻る

**配置例：** Room1-6 などの初期Room

---

### Enemy3（強敵）
| 属性 | 値 |
|------|-----|
| HP | 15 |
| 移動速度 | 4 |
| 検知範囲 | 10 |
| ダメージ | 3 |
| 重さ（mass） | 2 |
| 特徴 | 重い、広い検知範囲、高速 |

**行動パターン：**
- プレイヤーを検知 → Alert 状態（1秒ジャンプ + びっくりマーク）
- Chase 状態で高速追跡
- 検知範囲（10 ユニット）が広いため遠くから追ってくる
- moveRange > 10 で見失う

**配置例：** Room3（create enemy3: 134, 1, 0）

---

### Enemy4（ボス敵）
| 属性 | 値 |
|------|-----|
| HP | 50 |
| 移動速度 | 2 |
| 検知範囲 | 5 |
| ダメージ | 3 |
| 重さ（mass） | 0.5 |
| 特徴 | 高HP、重力あり、天井から降下 |

**行動パターン：**
- Rigidbody2D.gravityScale = 1 で重力有効
- 重力により天井から落下してくる
- Y位置のみ追跡、X は固定
- プレイヤーのX座標と同じ位置で待機

**配置例：** Room8 中央、天井上（create enemy4: 280, 20, 0）

---

## 📐 敵タイプの選択基準

| 状況 | 推奨敵 | 理由 |
|------|-------|------|
| 初期Room（広い） | Enemy2D | 弱めで学習用 |
| 中盤Room | Enemy3 | 難易度上昇 |
| 特殊配置（天井から） | Enemy4 | 独特の挙動 |
| 複数配置 | Enemy2D | 計算負荷低減 |

---

## 🔧 敵作成EditorScript パターン

### 基本構造（Enemy2D2Creator.cs）
```csharp
[MenuItem("Tools/Create Enemy<N>")]
public static void CreateEnemy<N>()
{
    // 既存敵削除
    GameObject existingEnemy = GameObject.Find("Enemy<N>");
    if (existingEnemy != null)
    {
        Object.DestroyImmediate(existingEnemy);
    }

    // オブジェクト作成
    GameObject obj = new GameObject("Enemy<N>");
    obj.AddComponent<Rigidbody2D>();
    obj.AddComponent<BoxCollider2D>();
    obj.AddComponent<SpriteRenderer>();
    obj.AddComponent<Enemy<N>>();

    // 設定
    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
    rb.gravityScale = 1f;
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    Enemy<N> enemy = obj.GetComponent<Enemy<N>>();
    enemy.hp = <hp値>;
    enemy.moveSpeed = <speed>;
    enemy.damageAmount = <damage>;
    enemy.moveRange = <range>;

    // 配置
    obj.transform.position = new Vector3(<x>, <y>, 0);
    obj.tag = "enemy";
    
    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    Debug.Log("Enemy<N> を作成しました");
}
```

---

## ✅ 新しい敵を追加する時のチェックリスト

- [ ] `Enemy<N>` クラスを作成（EnemyBase 継承）
- [ ] `HandleChase()` をオーバーライド
- [ ] `damageAmount` をインスペクターで編集可能に設定
- [ ] `Enemy<N>Creator.cs` を作成
- [ ] `[MenuItem("Tools/Create Enemy<N>")]` を指定
- [ ] 既存敵削除機能を実装
- [ ] Rigidbody2D/BoxCollider2D 設定
- [ ] タグを "enemy" に設定
- [ ] EditorSceneManager.MarkSceneDirty() で保存マーク
- [ ] インスペクターで hp、moveSpeed、damageAmount、moveRange を設定

---

## 📌 敵配置の推奨パターン

### Room ごとの敵配置例
- **Room1**: Enemy2D × 1 （位置: 20, 1, 0）
- **Room3**: Enemy3 × 1 （位置: 134, 1, 0） 
- **Room8**: Enemy4 × 1 （位置: 280, 20, 0）

### ダメージバランス
- **Player HP**: 20
- **敵ダメージ**: 3
- **敵複数配置**: 無敵時間（1秒）で対策

---

## 🚀 次の敵タイプ追加の予想構成

例：Enemy5 を追加する場合
```
敵タイプ: Enemy5
HP: 20 （Enemy3より強い）
moveSpeed: 3
damageAmount: 3～4
moveRange: 8
特徴: 中間難易度

EditorScript: Enemy5Creator.cs
配置コマンド: Tools > Create Enemy5
```

---

## ⚠️ 敵作成時の重要ルール

### メソッドオーバーライド前の確認

新しい敵タイプで親クラス（EnemyBase）のメソッドをオーバーライドする場合：

**1. 親クラスを先に確認する**
```csharp
// EnemyBase.cs でメソッドをチェック
protected virtual void OnTriggerEnter2D(Collider2D other)  // ✅ virtual あり
protected void HandleChase()  // ✅ virtual あり（オーバーライド用）

// ❌ virtual キーワードがない場合は修正が必要
```

**2. virtual キーワードがない場合**
- 親クラス（EnemyBase）を修正して virtual を追加
- その後、派生クラスを作成する
- 先に派生クラスを作るとコンパイルエラー

**3. 影響範囲確認**
修正後、他の派生クラスに影響がないか確認：
- Enemy2D、Enemy3、Enemy4 など既存敵が影響を受けないか
- 既存敵が該当メソッドを使用していないか確認

### チェックリスト（追加項目）

敵作成時：
- [ ] EnemyBase のオーバーライド対象メソッドを先に確認
- [ ] **virtual キーワードが付いているか確認**
- [ ] **virtual がなければ、親クラスを修正してから派生クラス作成**
- [ ] 他の派生クラスへの影響を確認
- [ ] コンパイルエラーがないか確認

### 例：EnemyShield の場合

❌ **やってはいけない順序**
```
1. EnemyShield.cs を作成
2. OnTriggerEnter2D をオーバーライド
3. → コンパイルエラー（virtual がない）
4. → 慌てて EnemyBase を修正
```

✅ **正しい順序**
```
1. EnemyBase.cs の OnTriggerEnter2D を確認
2. virtual がないなら先に追加
3. 他の敵に影響ないか確認
4. → その後 EnemyShield.cs を作成
```

---

**ルール作成日**: 2026-06-24
**ルール更新日**: 2026-06-29（メソッドオーバーライド確認ルール追加）
