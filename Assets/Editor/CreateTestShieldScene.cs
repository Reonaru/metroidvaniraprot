using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// テストシーン「TestShield.unity」構築用エディタスクリプト
///
/// === 実行手順 ===
/// 1. Assets/Editor/CreateTestShieldScene.cs が存在することを確認
/// 2. Unity メニュー → Window → Create Test Shield Scene を選択
/// 3. ダイアログで「Create」ボタンを押下
/// 4. Assets/Scenes/TestShield.unity が生成される
///
/// === シーン構成 ===
/// - Main Camera: シーンビュー兼 ゲームビュー用カメラ
/// - Player: テスト用プレイヤー（bullet 発射スクリプト必須）
/// - EnemyShield: 盾役敵（1体配置）
/// - Ground: 地面（Collider2D）
/// - Canvas: デバッグUI（テスト進捗表示）
/// </summary>
public class CreateTestShieldScene
{
    [MenuItem("Window/Create Test Shield Scene")]
    public static void CreateScene()
    {
        if (EditorUtility.DisplayDialog(
            "テストシーン作成",
            "Assets/Scenes/TestShield.unity を新規作成します。\n既存ファイルがある場合は上書きされます。",
            "Create",
            "Cancel"))
        {
            BuildTestShieldScene();
        }
    }

    private static void BuildTestShieldScene()
    {
        // 新規シーンを作成
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // === Camera 設定 ===
        var cameraObj = new GameObject("Main Camera");
        var camera = cameraObj.AddComponent<Camera>();
        camera.backgroundColor = Color.black;
        var cameraTransform = cameraObj.GetComponent<Transform>();
        cameraTransform.position = new Vector3(0, 0, -10);

        cameraObj.AddComponent<AudioListener>();
        cameraObj.tag = "MainCamera";

        // === Ground 設定（地面） ===
        var groundObj = new GameObject("Ground");
        var groundTransform = groundObj.GetComponent<Transform>();
        groundTransform.position = new Vector3(0, -3, 0);

        var groundSprite = groundObj.AddComponent<SpriteRenderer>();
        groundSprite.color = new Color(0.5f, 0.3f, 0.2f, 1f);  // 茶色

        var groundCollider = groundObj.AddComponent<BoxCollider2D>();
        groundCollider.size = new Vector2(15f, 1f);
        groundCollider.tag = "ground";

        var groundRb = groundObj.AddComponent<Rigidbody2D>();
        groundRb.isKinematic = true;
        groundRb.constraints = RigidbodyConstraints2D.FreezeAll;

        // === Player 設定 ===
        var playerObj = new GameObject("Player");
        playerObj.tag = "Player";

        var playerTransform = playerObj.GetComponent<Transform>();
        playerTransform.position = new Vector3(-4, -2, 0);
        playerTransform.localScale = new Vector3(1, 1.5f, 1);

        var playerSprite = playerObj.AddComponent<SpriteRenderer>();
        playerSprite.color = Color.blue;

        var playerCollider = playerObj.AddComponent<BoxCollider2D>();
        playerCollider.size = new Vector2(0.6f, 1.2f);

        var playerRb = playerObj.AddComponent<Rigidbody2D>();
        playerRb.gravityScale = 1f;
        playerRb.mass = 1f;

        // === Enemy Shield 設定 ===
        var enemyObj = new GameObject("EnemyShield");
        var enemyTransform = enemyObj.GetComponent<Transform>();
        enemyTransform.position = new Vector3(3, -2, 0);

        var enemySprite = enemyObj.AddComponent<SpriteRenderer>();
        enemySprite.color = Color.red;

        var enemyCollider = enemyObj.AddComponent<BoxCollider2D>();
        enemyCollider.size = new Vector2(0.8f, 1.2f);
        enemyCollider.isTrigger = false;

        var enemyRb = enemyObj.AddComponent<Rigidbody2D>();
        enemyRb.gravityScale = 1f;
        enemyRb.mass = 1f;

        var enemyBase = enemyObj.AddComponent<EnemyShield>();
        enemyBase.hp = 3;
        enemyBase.moveSpeed = 2f;
        enemyBase.mass = 1f;
        enemyBase.roomID = "TestRoom";
        enemyBase.damageAmount = 1f;
        enemyBase.moveRange = 6f;
        enemyBase.groundLayer = LayerMask.GetMask("Default");
        enemyBase.StageLayer = LayerMask.GetMask("Default");
        enemyBase.turnCooldown = 0.5f;

        // 盾オブジェクト（Enemy の子オブジェクト）
        var shieldVisualObj = new GameObject("Shield");
        shieldVisualObj.transform.SetParent(enemyObj.transform);
        shieldVisualObj.transform.localPosition = new Vector3(0.5f, 0, 0);

        var shieldSprite = shieldVisualObj.AddComponent<SpriteRenderer>();
        shieldSprite.color = new Color(0.2f, 0.8f, 1f, 0.7f);  // 水色半透明

        var shieldRenderer = shieldVisualObj.GetComponent<SpriteRenderer>();
        shieldRenderer.sortingOrder = -1;

        enemyBase.shieldObject = shieldVisualObj;

        // Ground Check
        var groundCheckObj = new GameObject("GroundCheck");
        groundCheckObj.transform.SetParent(enemyObj.transform);
        groundCheckObj.transform.localPosition = new Vector3(0, -0.6f, 0);
        enemyBase.groundCheck = groundCheckObj.transform;

        // Wall Check
        var wallCheckObj = new GameObject("WallCheck");
        wallCheckObj.transform.SetParent(enemyObj.transform);
        wallCheckObj.transform.localPosition = new Vector3(0.4f, 0, 0);
        enemyBase.wallCheck = wallCheckObj.transform;

        // === Canvas / Debug UI ===
        var canvasObj = new GameObject("Canvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        var graphicsRaycaster = canvasObj.AddComponent<GraphicRaycaster>();

        // Debug Text
        var textObj = new GameObject("DebugText");
        textObj.transform.SetParent(canvasObj.transform);

        var rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = new Vector2(300, 150);

        var text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = "TestShield シーン\n\nプレイヤー (青): 弾を発射\n敵シールド (赤): 盾で防ぐ\n\n盾破壊までの攻撃回数: 3回";
        text.alignment = TextAnchor.UpperLeft;
        text.fontSize = 14;
        text.color = Color.white;

        // === シーンを保存 ===
        string scenePath = "Assets/Scenes/TestShield.unity";
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        EditorUtility.DisplayDialog(
            "完了",
            $"テストシーンを作成しました。\n{scenePath}",
            "OK");

        Debug.Log($"✓ TestShield.unity を作成: {scenePath}");
    }
}
