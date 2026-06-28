using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom10Triggers
{
    [MenuItem("Tools/Create Room10 Triggers")]
    public static void CreateTriggers()
    {
        // Room10 の境界線
        float leftBoundaryX = 340f;
        float rightBoundaryX = 358f;
        float triggerY = -10f;
        float triggerHeight = 10f;

        // RoomData を読み込む
        RoomData room9 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room9.asset");
        RoomData room11 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room11.asset");

        // 既存トリガーを削除
        GameObject existingLeftTrigger = GameObject.Find("Room10_LeftTrigger");
        if (existingLeftTrigger != null)
        {
            Object.DestroyImmediate(existingLeftTrigger);
            Debug.Log("既存の Room10_LeftTrigger を削除しました");
        }

        GameObject existingRightTrigger = GameObject.Find("Room10_RightTrigger");
        if (existingRightTrigger != null)
        {
            Object.DestroyImmediate(existingRightTrigger);
            Debug.Log("既存の Room10_RightTrigger を削除しました");
        }

        // 左端トリガー（Room10→Room9）
        CreateTrigger("Room10_LeftTrigger", leftBoundaryX + 0.25f, triggerY, triggerHeight, room9);

        // 右端トリガー（Room10→Room11）
        if (room11 != null)
        {
            CreateTrigger("Room10_RightTrigger", rightBoundaryX - 0.25f, triggerY, triggerHeight, room11);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room10 スクロールトリガーを作成しました");
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
