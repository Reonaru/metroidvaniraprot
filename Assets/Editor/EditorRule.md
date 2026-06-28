# EditorScript ルール

## 📋 概要

EditorScript は開発時に使用するツール。ゲーム実行時には不要。
責任は「開発者の意図をUnityに反映させる」こと。

---

## 📐 ルーム設計ルール

### 部屋のサイズ
- **基本幅**: X 軸の幅は 40 ユニット
- **拡張時**: 40 の倍数で広げる（例：80, 120, 160...）
- 例：Room1-6 は幅 40、Room7 は幅 40、Room8 は幅 40

### 新しい Room 追加時の Y 軸設定
- **基本**: YBoundary = 0, maxYBoundary = 0（カメラ高さ固定）
- **隣接 Room と異なる Y 設定を持つ場合**：
  - 設定の理由を明確にする（カメラが上下に動く場面があるか など）
  - **隣接 Room と同じ Y 設定にするのがデフォルト**
  - 例：Room8 が YBoundary = -15, maxYBoundary = 10 なら、Room9 も同じ設定

**例：**
- Room8 (特殊な Y 設定: -15～10) → Room9 を隣に作成 → Room9 も -15～10 に設定
- Room1 (標準設定: 0～0) → Room2 を隣に作成 → Room2 も 0～0 に設定

### スクロールトリガー配置
- **位置**: 境界線の前後 ±0.25 に配置（**重要：配置順序は固定**）
  - 進む方向のトリガー（RightTrigger）: `boundaryX - 0.25`
  - 戻る方向のトリガー（LeftTrigger）: `boundaryX + 0.25`
- **Y座標**: Room ごとに異なる場合がある
  - 標準設定: Y = 0
  - 特殊設定: Y = -10（Room8, Room9 など）
- **高さ**: 10 ユニット（プレイ領域全体をカバー）

**なぜこの配置か：**
- プレイヤーが右に移動して進む方向トリガー（-0.25）に先に触れる
- その後、戻る方向トリガー（+0.25）はプレイヤーが戻ってくるときだけ触れる
- 両トリガーが同時に発動するのを防ぐ

**例：Room4-Room7 境界線（X=220）の場合**
```
┌─────────────────────────┐
│ Room4                   │
│                      219.75 ← Room4_RightTrigger (Y=0)
│                   220 (boundary)
│                      220.25 ← Room7_LeftTrigger (Y=0)
│                         Room7 │
└─────────────────────────┘
```
- Room4_RightTrigger: X = 219.75, Y = 0（Room4 → Room7）
- Room7_LeftTrigger: X = 220.25, Y = 0（Room7 → Room4）

---

## 🎯 命名規則

### クラス名
- `Create<ObjectName>.cs` - オブジェクト生成用
- `Diagnostic<Feature>.cs` - 診断・検査用
- 例: `CreateEnemy2D2.cs`, `DiagnosticTilemapBounds.cs`

### メニュー項目
- `[MenuItem("Tools/Create <ObjectName>")]`
- `[MenuItem("Tools/Diagnostic/<Feature>")]`

---

## 🔄 既存オブジェクトのリセット

**⚠️ 必須ルール：オブジェクト作成前に既存オブジェクトを削除**

EditorScript でオブジェクトを作成する場合、**メニューを実行するたびに既存オブジェクトを削除してから新規作成すること**。

```csharp
// 既存オブジェクトを削除してから新規作成
GameObject existing = GameObject.Find("ObjectName");
if (existing != null)
{
    Object.DestroyImmediate(existing);
    Debug.Log("既存の ObjectName を削除しました");
}

// その後、新規作成・設定
// ...
```

**理由:**
- 複数回実行時に重複を防ぐ
- 設定変更を反映させやすくする
- 古い設定値で残ったオブジェクトによるバグを防止

---

## 📌 実装パターン

### 1. シンプル作成パターン（Enemy2D2Creator.cs）
```csharp
[MenuItem("Tools/Create <Name>")]
public static void Create<Name>()
{
    // 既存オブジェクト削除
    // オブジェクト作成
    // コンポーネント追加
    // 設定値を代入
    // シーン保存マーク
    // ログ出力
}
```

### 2. 複数トリガーパターン（CreateRoom4Triggers.cs）
```csharp
[MenuItem("Tools/Create Room<N> Triggers")]
public static void CreateTriggers()
{
    // RoomData を読み込む
    RoomData roomX = AssetDatabase.LoadAssetAtPath<RoomData>("...");
    
    // 複数トリガーを作成
    CreateTrigger("Trigger_Left", x1, y, height, targetRoom);
    CreateTrigger("Trigger_Right", x2, y, height, targetRoom);
}

static void CreateTrigger(string name, float x, float y, float height, RoomData target)
{
    // 共通処理
}
```

### 3. シャッター作成パターン（CreateShatter.cs）
```csharp
[MenuItem("Tools/Create Shatter")]
public static void CreateShatterObject()
{
    // 既存シャッター削除
    GameObject existingShatter = GameObject.Find("Shatter");
    if (existingShatter != null)
    {
        Object.DestroyImmediate(existingShatter);
    }

    // オブジェクト作成
    GameObject shatterObj = new GameObject("Shatter");
    
    // Collider 2つ追加（重要）：
    // 1. isTrigger=false - Player との物理衝突を受ける
    // 2. isTrigger=true - bullet の Trigger 判定を受ける
    BoxCollider2D boxCollider = shatterObj.AddComponent<BoxCollider2D>();
    boxCollider.isTrigger = false;  // Player 衝突用
    
    BoxCollider2D triggerCollider = shatterObj.AddComponent<BoxCollider2D>();
    triggerCollider.isTrigger = true;  // bullet 判定用
    
    // DoorController 追加（弾を受け取る機能を持つ）
    shatterObj.AddComponent<DoorController>();
    
    // タグ設定（必須：bullet.cs が "shatter" タグで判定）
    shatterObj.tag = "shatter";
}
```

**シャッター作成時の注意点：**
- **Collider 2つが必須**：Player との物理衝突と bullet の Trigger 判定の両方に対応
- **タグは "shatter" に統一**：bullet.cs が このタグで判定するため
- **DoorController 使用**：弾5発でドア開く、揺れ演出などの機能を持つ
- **複数配置可能**：Tools > Create Shatter を複数回実行すれば複数配置できる

---

## ✅ チェックリスト

新しい EditorScript を作成する際：

- [ ] クラス名が `Create<Name>` または `Diagnostic<Name>` 形式
- [ ] `[MenuItem("Tools/...")]` が指定されている
- [ ] 既存オブジェクトをリセット（必要に応じて）
- [ ] ScriptableObject/Prefab 参照は `AssetDatabase.LoadAssetAtPath()` を使用
- [ ] 作成後に `EditorSceneManager.MarkSceneDirty()` で保存マーク
- [ ] 作成完了時に `Debug.Log()` でメッセージ出力
- [ ] コンパイルエラーがないか確認

---

## 📁 関連ファイル一覧

| ファイル | 機能 | メニュー |
|---------|------|---------|
| Enemy2D2Creator.cs | Enemy2D2 敵生成 | Tools > Create Enemy2D2 |
| Enemy3Creator.cs | Enemy3 敵生成 | Tools > Create Enemy3 |
| Enemy4Creator.cs | Enemy4 敵生成 | Tools > Create Enemy4 |
| CreateDebugCameraUI.cs | デバッグUI生成 | Tools > Create Debug Camera UI |
| CreateHPBar.cs | HPゲージUI生成 | Tools > Create HP Bar |
| CreateItem.cs | アイテム生成 | Tools > Create Item |
| CreateShatter.cs | シャッター生成 | Tools > Create Shatter |
| DiagnosticTilemapBounds.cs | Tilemap診断 | Tools > Diagnostic > Inspect Tilemap Bounds |
| CreateRoom4Triggers.cs | Room4 トリガー | Tools > Create Room4 Triggers |
| CreateRoom7Triggers.cs | Room7 トリガー | Tools > Create Room7 Triggers |
| CreateRoom8Triggers.cs | Room8 トリガー | Tools > Create Room8 Triggers |
| CreateRoom9Triggers.cs | Room9 トリガー | Tools > Create Room9 Triggers |

---

## 🚀 Room追加パターン（新しい部屋を作る時）

### 現在のマップ構成
```
Room1-6: X 0～(各40単位)
Room4: X 180～220
Room7: X 220～260
Room8: X 260～300 (Y: -15～10)
Room9: X 300～340 (Y: -15～10)
Room10: X 340～358 (Y: -15～10, 画面幅17.8)
Room11: X 358～398 (Y: -15～10)
```

### 新しい Room を追加する時の手順

1. **X座標を決定**
   - 前のRoom の maxXBoundary が新しいRoom の minXBoundary
   - 例：Room11 が 358～398 なら、Room12 は 398～438

2. **RoomData を作成**
   ```csharp
   [MenuItem("Tools/Create Room<N> Data")]
   public static void CreateRoom<N>Asset()
   {
       RoomData room<N> = ScriptableObject.CreateInstance<RoomData>();
       room<N>.roomID = <N>;
       room<N>.minXBoundary = <前のRoom.maxXBoundary>;
       room<N>.maxXBoundary = <minXBoundary + 幅>;
       room<N>.YBoundary = -15;  // Room8以降のデフォルト
       room<N>.maxYBoundary = 10;
       room<N>.scrollType = RoomData.ScrollType.Horizontal;
       // ... 保存処理
   }
   ```

3. **前のRoom のトリガーを修正**
   - CreateRoom<N-1>Triggers.cs で RightTrigger を追加

4. **新しいRoom のトリガーを作成**
   - CreateRoom<N>Triggers.cs で LeftTrigger を作成

### 次に作るRoom12 の予想構成
```
roomID: 12
minXBoundary: 398
maxXBoundary: 438
YBoundary: -15
maxYBoundary: 10
幅: 40
スクロール: 水平
```

---

## 🚀 今後の拡張

新しい Room（Room8, Room9...）を追加する際は、同じパターンで EditorScript を作成：

```
CreateRoom<N>Triggers.cs
├─ 境界線を変数化
├─ 隣接 Room を参照
└─ トリガー自動生成
```

---

**ルール作成日**: 2026-06-16
**最終更新**: 2026-06-24
