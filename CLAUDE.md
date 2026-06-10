# Metroidvania Game Project

## Project Overview
A 2D Metroidvania-style game developed in Unity. The project includes player mechanics, enemy AI, boss battles, room management, and game progression systems.

## Current Build Status
**Last Commit:** prottype metroidvanira (6357fd5)
**Active Branch:** main

## Key Game Systems

### Player System
- **File:** `Assets/Script/Playersc.cs`
- **Status:** In Development
- **Features:** Player movement, jumping, combat mechanics

### Enemy & Boss System
- **Files:** `Assets/Script/Enemy2D.cs`, `Assets/Boss.cs`
- **Status:** In Development
- **Features:** Enemy AI, patrol logic, boss mechanics

### Room Management
- **Files:** `Assets/RoomManager.cs`, `Assets/RoomMember.cs`, `Assets/roomidscri.cs`
- **Status:** In Development
- **Features:** Room transitions, room-based level design

### UI System
- **Files:** `Assets/Script/GameOverUi.cs`, `Assets/GameOver.cs`
- **Status:** Recently Added
- **Features:** Game over screen, UI management

### Camera System
- **File:** `Assets/Script/CameraManager1.cs`
- **Status:** In Development
- **Features:** Camera following, scene management

### Interaction System
- **Files:** `Assets/droparea.cs`, `Assets/BlockActivator.cs`
- **Status:** In Development
- **Features:** Interactive blocks, drop zones

## File Structure
```
Assets/
├── Script/              # Main game scripts
├── prefab/              # Prefabs (Player, bullets)
├── Object/BGM/SE/       # Audio assets
├── MMDevelopers/        # DarkWolf animation assets
└── Scenes/              # Game scenes
```

## Modified Files (Pending Review)
- BlockActivator.cs
- Boss.cs
- RoomManager.cs, RoomMember.cs
- CameraManager1.cs, Enemy2D.cs
- PlayerCollision.cs, Playersc.cs
- Various animation controllers and assets

## New Files (Not Yet Committed)
- GameOverUi.cs
- GameOver.cs
- droparea.cs
- Player.prefab
- Noto_Sans_JP font assets
- Layer Lab resources

## Development Notes
- Using TextMesh Pro for UI
- DarkWolf_2d character with animations
- Metriovania-style progression system

## Camera System Refactoring

カメラシステムのリファクタリング予定があります。詳細は `/Users/unity_dev/Unipro/list/Refactoring_Plan.md` を参照してください。

主な目標：今後のカメラギミック追加に対応できるように、現在の構成を改善する。

## Claude Code 運用ルール

### ディレクトリ操作について
- **重要:** gitの情報が正確だと思い込まない。gitのメタデータ（コミット日時、ブランチ状態）は信頼できない。
- 実ファイルの状態を分析する：タイムスタンプ、内容、ディレクトリ構造だけが真実。
- バージョン比較や変更内容を確認するときは、実ファイルのタイムスタンプ（`ls -l`）、内容（`diff`）などで確認すること。

### git使用について
- **厳格なルール:** gitコマンドはユーザーの明示的な許可なしには絶対に実行禁止。
- git log, git status, git diff, git push など、どのgitコマンドでも実行する前に、ユーザーに許可を求めて了承を得る必須。
- ユーザーがgit操作を拒否した場合は、絶対にgit操作をしない。

### 作業記録の管理について

#### WORKLOG.md（作業履歴表）
- **ルール:** 作業終了後、必ずその日の作業内容を記録する
- 記入内容：時刻、タスク番号、実施内容、状態、備考
- 毎日のまとめ：作業時間、完了タスク数、進捗状況、次の予定

#### PROCESSING.md（進捗管理表）
- **ルール:** 作業終了後、プロジェクト全体の進捗を更新する
- 更新内容：全体進捗率、コア機能の完了度、タスク一覧の状態、ブロッカーの記録

#### 記録のタイミング
- **作業後の定義:** ユーザーが最後に指示してから30分経って、何もやることがない状態
- その時点で WORKLOG.md と PROCESSING.md を更新する
- 未解決の問題がある場合は問題項目に記載
- コミットが必要な場合はユーザーに相談

