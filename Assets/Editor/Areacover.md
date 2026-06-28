# Area Coverage Map

新しいオブジェクト作成を依頼された場合、**必ず最初にこのドキュメントを確認する**。

---

## 📐 座標規則（静的・変わらない）

### Room サイズ規則
- **基本幅**: 40 ユニット
- **拡張時**: 40 の倍数で拡張
- **連続性**: 前の Room の `maxXBoundary` = 次の Room の `minXBoundary`

**例：**
```
Room10: minX = 340, maxX = 358 (幅 17.8)
Room11: minX = 358, maxX = 398 (幅 40)  ← 340 ≠ 358 なので、358 から開始
```

### Y 座標規則
- **Room1～7**: YBoundary = 0, maxYBoundary = 0（カメラ固定）
- **Room8～**: YBoundary = -15, maxYBoundary = 10（デフォルト、隣接 Room と揃える）

### トリガー配置ルール
- **進む方向**: `boundaryX - 0.25`（例：X=300 の境界なら 299.75）
- **戻る方向**: `boundaryX + 0.25`（例：X=300 の境界なら 300.25）
- **順序固定**: 進む方向トリガーが先、戻る方向が後
- **理由**: プレイヤーが両トリガーに同時に触れるのを防ぐ

---

## 🔍 現状把握（動的・RoomData が唯一の真実）

### ⚠️ 重要
**手書きの Room 一覧は作らない。RoomData ファイルを直接読む。**

現在の Room 状態を確認する場合：
1. `Assets/scriptable/` ディレクトリを確認
2. `room1.asset` ～ `room11.asset` を読み込む
3. 各 RoomData の以下をチェック：
   - `roomID`
   - `minXBoundary`
   - `maxXBoundary`
   - `YBoundary`
   - `maxYBoundary`
   - `scrollType`

**新しい Room を追加する場合、ここから最新情報を取得する。**

---

## 📋 作業手順（部屋追加時）

### ステップ 1: 現状把握
```
現在の全 RoomData を読み込む
  ↓
最後の Room の maxXBoundary を確認
  ↓
次の Room の minXBoundary = 前 Room の maxXBoundary
```

### ステップ 2: 座標規則に従って次の値を決定
```
次の Room:
  - minXBoundary: 前 Room の maxXBoundary
  - maxXBoundary: minXBoundary + 40（または指定幅）
  - YBoundary: 隣接 Room と同じ（Room8以降は -15）
  - maxYBoundary: 隣接 Room と同じ（Room8以降は 10）
```

### ステップ 3: 部屋生成ルール.md に従って生成
```
1. CreateRoom<N>Data.cs で RoomData 作成
2. CreateRoom<N-1>Triggers.cs で RightTrigger 追加
3. CreateRoom<N>Triggers.cs で LeftTrigger 作成
4. EditorScript を実行して生成
5. Areacover.md の「現状把握」セクションに行を追加（最初は空欄で OK）
```

---

## 📍 参考情報（最後に確認した状態）

⚠️ このセクションは参考用。実際には `Assets/scriptable/roomX.asset` を読む。

### Room座標一覧（最後の確認）

| Room | minX | maxX | 幅 | Y境界 | maxY | スクロール | 特徴 |
|------|------|------|-----|-------|------|-----------|------|
| Room1 | 0 | 40 | 40 | 0 | 0 | 水平 | 開始地点 |
| Room2 | 40 | 80 | 40 | 0 | 0 | 水平 | 通常 |
| Room3 | 80 | 120 | 40 | 0 | 0 | 水平 | Enemy3 配置 |
| Room4 | 180 | 220 | 40 | 0 | 0 | 水平 | 通常 |
| Room5 | （未確定） | （未確定） | 40 | 0 | 0 | 水平 | 通常 |
| Room6 | （未確定） | （未確定） | 40 | 0 | 0 | 水平 | 通常 |
| Room7 | 220 | 260 | 40 | 0 | 0 | 水平 | 通常 |
| Room8 | 260 | 300 | 40 | -15 | 10 | 水平 | 敵強化、カメラ上下動 |
| Room9 | 300 | 340 | 40 | -15 | 10 | 水平 | 敵強化 |
| Room10 | 340 | 358 | 17.8 | -15 | 10 | 水平 | 画面幅調整 |
| Room11 | 358 | 398 | 40 | -15 | 10 | 水平 | 敵強化 |

**座標注記：**
- Y = 0 標準設定（Room1～7）
- Y = -15～10 特殊設定（Room8～11、カメラ上下動あり）

---

## 🚪 シャッター配置

| Room | 名前 | 位置 | 破壊条件 | 目的 | 演出 |
|------|------|------|---------|------|------|
| Room1 | room1_shatter | （TBD） | 弾 5 発 | **チュートリアル**：破壊方法を学ぶ | 揺れ |
| Room8 | room8_shatter | （TBD） | 弾 5 発 | 進行ゲート | 揺れ |

**タイプ：** 全て DoorController（ダメージベース）
**タグ：** "shatter"
**実装：** CreateShatter.cs で生成

---

## 👾 敵配置

### Enemy2D（基本敵）
| Room | 位置 | HP | 速度 | 検知範囲 | ダメージ | 特徴 |
|------|------|----|----|---------|---------|------|
| （未確定） | （未確定） | 3 | 2 | 5 | 3 | 軽い |

### Enemy3（強敵）
| Room | 位置 | HP | 速度 | 検知範囲 | ダメージ | 特徴 |
|------|------|----|----|---------|---------|------|
| Room3 | (134, 1, 0) | 15 | 4 | 10 | 3 | 高速、広範囲 |

### Enemy4（ボス敵）
| Room | 位置 | HP | 速度 | 検知範囲 | ダメージ | 特徴 |
|------|------|----|----|---------|---------|------|
| Room8 | (280, 20, 0) | 50 | 2 | 5 | 3 | 重力あり、天井から降下 |

---

## 📊 Room 難易度目安

```
Room1～2: ⭐ 簡単（敵なし/少なし）
Room3～4: ⭐⭐ 通常
Room5～7: ⭐⭐ 通常
Room8～11: ⭐⭐⭐ 難しい（Y動作、強い敵）
```

---

## ⚙️ 新規作成時の確認項目

### 新 Room を追加する場合
- [ ] minXBoundary を前 Room の maxXBoundary に合わせる
- [ ] Room のこのドキュメントに行を追加
- [ ] CreateRoom<N>Data.cs で RoomData を作成
- [ ] CreateRoom<N-1>Triggers.cs を修正（RightTrigger 追加）
- [ ] CreateRoom<N>Triggers.cs を作成（LeftTrigger 作成）

### 新シャッターを配置する場合
- [ ] 配置 Room の座標を確認
- [ ] シャッターの目的を定義
- [ ] CreateShatter.cs で生成
- [ ] このドキュメントに行を追加

### 新敵を配置する場合
- [ ] 配置 Room の座標を確認
- [ ] 敵タイプを選択（Enemy2D / Enemy3 / Enemy4）
- [ ] 敵のパラメータを確認（HP、速度、検知範囲）
- [ ] EditorScript で生成
- [ ] このドキュメントに行を追加

---

## 📝 更新履歴

| 日付 | 変更内容 |
|------|---------|
| 2026-06-24 | 初版作成、Room1～11 配置記録 |
| - | シャッター 2 個記録（Room1, Room8） |
| - | 敵配置 3 個記録（Room3, Room8） |

---

**最終更新**: 2026-06-24
**作成者**: Claude Code
