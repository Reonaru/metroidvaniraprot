# シャッター（ドア）生成ルール

## 📋 概要

シャッターは 2 つのオープンシステムに対応：
1. **フラグベース** - Game Manager のフラグで開く（shatter.cs）
2. **ダメージベース** - 弾で破壊して開く（DoorController.cs）

---

## 🎯 シャッタータイプ

### Type 1: フラグベース（shatter.cs）
| 属性 | 値 |
|------|-----|
| スクリプト | shatter.cs |
| 開閉条件 | フラグが true |
| 色 | グレー |
| 物理衝突 | Trigger のみ |
| 用途 | クエスト完了時の門など |

**動作：**
```csharp
void Update()
{
    bool isClear = Gmanager.Instance.GetFlag(targetFlagName);
    if (isClear)
    {
        OpenDoor();  // SetActive(false)
    }
}
```

**配置例：**
```csharp
shatter shatterScript = obj.AddComponent<shatter>();
shatterScript.targetFlagName = "Room1_Clear";
```

---

### Type 2: ダメージベース（DoorController.cs）
| 属性 | 値 |
|------|-----|
| スクリプト | DoorController.cs |
| 開閉条件 | 弾 5 発当たる |
| 演出 | 揺れ効果（Shake()） |
| 物理衝突 | あり（Player 通過不可） |
| ダメージ判定 | Trigger（bullet 判定） |
| 用途 | 戦闘要素のあるドア |

**動作：**
```csharp
void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("bullet"))
    {
        bulletHitCount++;
        if (bulletHitCount >= bulletHitThreshold)
        {
            OpenDoor();  // SetActive(false)
        }
    }
}
```

**カスタマイズ（インスペクター）：**
- `bulletHitThreshold`: ドア開き判定の弾数（デフォルト: 5）

---

## 🔧 シャッター作成EditorScript

### CreateShatter.cs（ダメージベース推奨）

```csharp
[MenuItem("Tools/Create Shatter")]
public static void CreateShatterObject()
{
    // 既存削除
    GameObject existingShatter = GameObject.Find("Shatter");
    if (existingShatter != null)
    {
        Object.DestroyImmediate(existingShatter);
    }

    // オブジェクト作成
    GameObject shatterObj = new GameObject("Shatter");
    shatterObj.transform.position = new Vector3(0, 0, 0);

    // SpriteRenderer
    SpriteRenderer spriteRenderer = shatterObj.AddComponent<SpriteRenderer>();
    spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f, 1f);  // 灰色

    // Collider 2つ必須
    // 1. 物理衝突（Player 通過不可）
    BoxCollider2D boxCollider = shatterObj.AddComponent<BoxCollider2D>();
    boxCollider.size = new Vector2(1f, 2f);
    boxCollider.isTrigger = false;  // 物理衝突

    // 2. Trigger 判定（bullet 検知）
    BoxCollider2D triggerCollider = shatterObj.AddComponent<BoxCollider2D>();
    triggerCollider.size = new Vector2(1f, 2f);
    triggerCollider.isTrigger = true;  // Trigger

    // Rigidbody2D
    Rigidbody2D rb = shatterObj.AddComponent<Rigidbody2D>();
    rb.bodyType = RigidbodyType2D.Static;
    rb.gravityScale = 0f;

    // DoorController 追加
    DoorController doorController = shatterObj.AddComponent<DoorController>();

    // タグ設定（重要：bullet.cs が "shatter" タグで判定）
    shatterObj.tag = "shatter";

    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    Debug.Log("シャッターを作成しました");
}
```

---

## ⚙️ シャッター配置方法

### 新規配置手順

1. **EditorScript で生成**
   ```
   Tools > Create Shatter
   ```

2. **位置調整（インスペクター）**
   - Transform.position で X, Y を設定

3. **ダメージベースの場合**
   - DoorController.bulletHitThreshold を調整（デフォルト: 5）

4. **複数配置**
   - メニューを複数回実行
   - 各シャッターを異なる位置に配置

### 配置例

```
Room1: Shatter at (150, 0, 0) - 5発で開く
Room5: Shatter at (210, -5, 0) - 3発で開く（調整済み）
```

---

## ✅ シャッター作成時のチェックリスト

- [ ] `Tools > Create Shatter` を実行
- [ ] 位置を調整（Transform.position）
- [ ] ダメージベースの場合、bulletHitThreshold を確認
- [ ] タグが "shatter" になっているか確認
- [ ] 2つの Collider があるか確認
  - [ ] isTrigger = false（物理衝突用）
  - [ ] isTrigger = true（bullet 判定用）
- [ ] Rigidbody2D が Static に設定されているか確認
- [ ] bullet.cs の onTriggerEnter2D で "shatter" タグを判定できるか確認

---

## 🎯 シャッタータイプの選択基準

| 状況 | 推奨タイプ | 理由 |
|------|-----------|------|
| クエスト完了後 | フラグベース | ゲームの進行状況に依存 |
| 戦闘ゲーム | ダメージベース | プレイヤースキルが必要 |
| スイッチ式 | フラグベース | イベント駆動 |
| 破壊要素 | ダメージベース | 演出付き（揺れ） |

---

## 🔄 bullet との連携

### bullet.cs の判定
```csharp
if (other.CompareTag("shatter"))
{
    // シャッターを揺らす
    DoorController door = other.GetComponent<DoorController>();
    if (door != null)
    {
        door.Shake();  // 揺れ演出
    }
    Destroy(gameObject);  // 弾消滅
}
```

**重要：** bullet が shatter タグを判定するため、シャッターのタグ設定は必須

---

## 🚀 次のシャッター追加の予想構成

```
シャッター名: DoorRoom10
位置: (356, -5, 0)
タイプ: ダメージベース（DoorController）
破壊条件: 弾 3 発
タグ: "shatter"
Collider: 2つ（物理 + Trigger）
```

---

## 📍 現在の配置メモ

| Room | シャッター名 | 位置 | 目的 | タイプ |
|------|-----------|------|------|--------|
| Room1 | room1_shatter | （TBD） | **チュートリアル**：弾でシャッター破壊を学ぶ | ダメージベース |
| Room8 | room8_shatter | （TBD） | 進行ゲート：破壊時に揺れ演出あり | ダメージベース |

---

## 📝 今後の配置予定

新しいシャッターを配置する際はここに記録：

| Room | シャッター名 | 位置 | 目的 | タイプ |
|------|-----------|------|------|--------|
| （未定） | （未定） | （未定） | （未定） | （未定） |

---

**ルール作成日**: 2026-06-24
**最終更新**: 2026-06-24
