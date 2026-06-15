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

### スクロールトリガー配置
- **位置**: 境界線の前後 ±0.25 に配置
  - 左側トリガー: `boundaryX + 0.25`
  - 右側トリガー: `boundaryX - 0.25`
- **Y座標**: 0（画面下部）
- **高さ**: 10 ユニット（プレイ領域全体をカバー）
- 例：Room4-Room7 の境界線が X=220 の場合
  - Room4_RightTrigger: X = 219.75
  - Room7_LeftTrigger: X = 220.25

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
| CreateDebugCameraUI.cs | デバッグUI生成 | Tools > Create Debug Camera UI |
| DiagnosticTilemapBounds.cs | Tilemap診断 | Tools > Diagnostic > Inspect Tilemap Bounds |
| CreateRoom4Triggers.cs | Room4 トリガー | Tools > Create Room4 Triggers |
| CreateRoom7Triggers.cs | Room7 トリガー | Tools > Create Room7 Triggers |

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
