# スクロール判定ルール（フラグベース）

Room 間の移動時に、縦スクロール・横スクロールを判定する方法。

## ルール

### 1. RoomData に ScrollType フラグを追加

```csharp
public enum ScrollType { Horizontal, Vertical }
public ScrollType scrollType = ScrollType.Horizontal;
```

### 2. StartScroll() でフラグに基づいて判定

```csharp
if (targetRoom.scrollType == RoomData.ScrollType.Vertical)
{
    // 縦スクロール：Y を目標値に移動、X は範囲内に調整
    moveDirection = (diffY > 0) ? ScrollDirection.Up : ScrollDirection.Down;
    finalY = targetRoom.YBoundary;
    finalX = Mathf.Clamp(camPos.x, targetRoom.minXBoundary, targetRoom.maxXBoundary);
}
else
{
    // 横スクロール：X を移動、Y は固定
    if (diffX > 0) {
        moveDirection = ScrollDirection.Right;
        finalX = targetRoom.minXBoundary;
    } else {
        moveDirection = ScrollDirection.Left;
        finalX = targetRoom.maxXBoundary;
    }
    finalY = camPos.y;  // Y は固定（スクロール時は Y を動かさない）
}
```

## なぜフラグベースか

### 従来の diffY 自動判定の問題

```csharp
// ❌ 問題のあるコード
float diffY = targetRoom.YBoundary - camPos.y;

if (Mathf.Abs(diffY) > 2.0f) 
{
    // 縦移動判定
}
```

**問題点：**
- diffY の閾値（2.0）判定が複雑
- YBoundary の値で誤判定が発生しやすい
- 例：YBoundary = -5、camPos.y = 0 の場合
  - diffY = -5 - 0 = -5
  - |−5| = 5 > 2.0 → **縦移動判定に誤判定**
  - カメラが Y = -5 に移動して、プレイヤーが見えなくなる

### フラグベースの利点

- ✅ 判定が明確で分かりやすい
- ✅ YBoundary の値に影響されない
- ✅ バグの可能性が低い
- ✅ 新しい Room 追加時、scrollType を指定するだけで OK

## 新しい Room 追加時の手順

1. **RoomData（*.asset）を作成**
   ```
   例：room9.asset
   ```

2. **scrollType を設定**
   - 横スクロール Room → `scrollType = Horizontal`（デフォルト）
   - 縦スクロール Room → `scrollType = Vertical`に変更

3. **YBoundary, maxYBoundary を設定**
   - Horizontal の場合：YBoundary = 0（カメラ高さ固定）
   - Vertical の場合：YBoundary = 下限、maxYBoundary = 上限

## 現在のプロジェクト状態

| Room | scrollType | YBoundary | maxYBoundary |
|------|-----------|-----------|--------------|
| 1-7 | Horizontal | 0 | 0 |
| 8 | Horizontal | -10 | 10 |

- 全 Room が横スクロール
- Room8 のみ、カメラが Y = -10～10 の範囲で上下可能
