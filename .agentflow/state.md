# AgentFlow State

# 課題

- 今の部屋に、プレイヤーを約2分苦戦させる雑魚連携の敵パターンを作る
  - 前提: プレイヤーは遠距離(弾を撃つ)。撃つだけで抜けられない設計にしたい
  - 構成: 役割の違う雑魚を複数体組み合わせる
    - 盾役: プレイヤーの弾を正面で防ぎ、後ろの敵を守る
    - 近接突進役: プレイヤーに寄って距離を詰めてくる
    - 後衛牽制役: 安全地帯から弾を撃ち、プレイヤーを動かし続ける
  - 狙い: 「どれから倒すか」の判断を迫る。盾を崩さないと後衛に届かない等
  - 既存構造に乗せる: enemyの基底クラスを継承し、行動はoverrideで分ける
  - 部屋境界(ScriptableObject)の範囲内でスポーンさせる

## 完了済み（Phase 1-2）

### ✓ Phase 1: 盾役敵仕様確定
- 防御範囲：敵の moveDirection 基準で前方180度（左右各90度）
- 盾耐久度：プレイヤー弾3回で破壊
- 破壊時動作：shieldObject が非表示 + 敵AI 継続
- ビジュアルフィードバック：敵本体が黄色フラッシュ

### ✓ Phase 2: EnemyShield 実装・改善完了
- **新規ファイル:** Assets/Script/EnemyShield.cs（約160行）
- **実装内容:**
  - EnemyBase を継承した敵クラス定義
  - OnTriggerEnter2D オーバーライドで盾判定
  - IsInShieldRange() で防御範囲判定（敵の moveDirection 基準）
  - HandleShieldBlock() で盾HP 減少・弾消去
  - BreakShield() で盾破壊処理
  - ShieldBreakEffect() で黄色フラッシュエフェクト
  - OnDrawGizmosSelected() で盾防御範囲を可視化
  - HandleChase() で敵AI（プレイヤー追跡）
- **改善内容:**
  - bullet コンポーネント参照削除（不要）
  - damageAmount を使用した統一的なダメージ処理
  - デバッグログを詳細化（盾HP、破壊時の敵名などを表示）
  - Gizmo 描画で盾防御範囲を視覚化（敵選択時に表示）
- **テスト計画策定:** Phase 3 の検証手順を詳細化

## 進行中（Phase 3）

- **Phase 3: テスト実行**
  - テストシーン（Assets/Scenes/TestShield.unity）の作成 ← **次のステップ**
  - プレイ確認と仕様検証
  - 数値調整（盾耐久度など）

## 未解決の課題（Phase 4）

- **近接突進役敵（EnemyRusher）**：未実装
- **後衛牽制役敵（EnemyRanged）**：未実装
- **複数敵パターン統合テスト**：未実装（盾役 + 近接役 + 牽制役の3体連携テスト）
