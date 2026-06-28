using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom8Triggers
{
    [MenuItem("Tools/Create Room8 Triggers")]
    public static void CreateTriggers()
    {
        // Room8 の境界線
        float leftBoundaryX = 260f;
        float rightBoundaryX = 300f;
        float leftTriggerY = 0f;
        float rightTriggerY = -10f;
        float triggerHeight = 10f;

        // RoomData を読み込む
        RoomData room7 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room7.asset");
        RoomData room9 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room9.asset");

        // 既存トリガーを削除
        GameObject existingLeftTrigger = GameObject.Find("Room8_LeftTrigger");
        if (existingLeftTrigger != null)
        {
            Object.DestroyImmediate(existingLeftTrigger);
            Debug.Log("既存の Room8_LeftTrigger を削除しました");
        }

        GameObject existingRightTrigger = GameObject.Find("Room8_RightTrigger");
        if (existingRightTrigger != null)
        {
            Object.DestroyImmediate(existingRightTrigger);
            Debug.Log("既存の Room8_RightTrigger を削除しました");
        }

        // 左端トリガー（Room8→Room7）- Y = 0
        CreateTrigger("Room8_LeftTrigger", leftBoundaryX + 0.25f, leftTriggerY, triggerHeight, room7);

        // 右端トリガー（Room8→Room9）- Y = -10
        CreateTrigger("Room8_RightTrigger", rightBoundaryX - 0.25f, rightTriggerY, triggerHeight, room9);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room8 スクロールトリガーを作成しました");
    }

    static void CreateTrigger(string name, float x, float y, float height, RoomData targetRoom)
    {
        GameObject triggerObj = new GameObject(name);
        triggerObj.transform.position = new Vector3(x, y, 0);

        BoxCollider2D collider = triggerObj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1f, height);

        Rigidbody2D rb = triggerObj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        ScrollTrigger scrollTrigger = triggerObj.AddComponent<ScrollTrigger>();
        scrollTrigger.targetRoom = targetRoom;

        Debug.Log($"トリガー作成: {name} at ({x}, {y}) -> {targetRoom.name}");
    }
}
