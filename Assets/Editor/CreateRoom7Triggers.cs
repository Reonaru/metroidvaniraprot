using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom7Triggers
{
    [MenuItem("Tools/Create Room7 Triggers")]
    public static void CreateTriggers()
    {
        // Room7 の境界線
        float leftBoundaryX = 220f;
        float rightBoundaryX = 260f;
        float triggerY = 0f;
        float triggerHeight = 10f;

        // RoomData を読み込む
        RoomData room4 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room4.asset");
        RoomData room8 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room8.asset");

        // 既存トリガーを削除
        GameObject existingLeftTrigger = GameObject.Find("Room7_LeftTrigger");
        if (existingLeftTrigger != null)
        {
            Object.DestroyImmediate(existingLeftTrigger);
            Debug.Log("既存の Room7_LeftTrigger を削除しました");
        }

        GameObject existingRightTrigger = GameObject.Find("Room7_RightTrigger");
        if (existingRightTrigger != null)
        {
            Object.DestroyImmediate(existingRightTrigger);
            Debug.Log("既存の Room7_RightTrigger を削除しました");
        }

        // 左端トリガー（Room7→Room4）- 境界線より0.25先
        CreateTrigger("Room7_LeftTrigger", leftBoundaryX + 0.25f, triggerY, triggerHeight, room4);

        // 右端トリガー（Room7→Room8）- 境界線より0.25手前
        CreateTrigger("Room7_RightTrigger", rightBoundaryX - 0.25f, triggerY, triggerHeight, room8);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room7 スクロールトリガーを作成しました");
    }

    static void CreateTrigger(string name, float x, float y, float height, RoomData targetRoom)
    {
        GameObject triggerObj = new GameObject(name);
        triggerObj.transform.position = new Vector3(x, y, 0);

        BoxCollider2D collider = triggerObj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1f, height);

        // Rigidbody2D の追加（OnTriggerEnter2D が動作するため）
        Rigidbody2D rb = triggerObj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        // ScrollTrigger スクリプトを追加して targetRoom を設定
        ScrollTrigger scrollTrigger = triggerObj.AddComponent<ScrollTrigger>();
        scrollTrigger.targetRoom = targetRoom;

        Debug.Log($"トリガー作成: {name} at ({x}, {y}) -> {targetRoom.name}");
    }
}
