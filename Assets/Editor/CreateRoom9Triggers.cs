using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom9Triggers
{
    [MenuItem("Tools/Create Room9 Triggers")]
    public static void CreateTriggers()
    {
        // Room9 の境界線
        float leftBoundaryX = 300f;
        float rightBoundaryX = 340f;
        float triggerY = -10f;
        float triggerHeight = 10f;

        // RoomData を読み込む
        RoomData room8 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room8.asset");
        RoomData room10 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room10.asset");

        // 既存トリガーを削除
        GameObject existingLeftTrigger = GameObject.Find("Room9_LeftTrigger");
        if (existingLeftTrigger != null)
        {
            Object.DestroyImmediate(existingLeftTrigger);
            Debug.Log("既存の Room9_LeftTrigger を削除しました");
        }

        GameObject existingRightTrigger = GameObject.Find("Room9_RightTrigger");
        if (existingRightTrigger != null)
        {
            Object.DestroyImmediate(existingRightTrigger);
            Debug.Log("既存の Room9_RightTrigger を削除しました");
        }

        // 左端トリガー（Room9→Room8）
        CreateTrigger("Room9_LeftTrigger", leftBoundaryX + 0.25f, triggerY, triggerHeight, room8);

        // 右端トリガー（Room9→Room10）
        if (room10 != null)
        {
            CreateTrigger("Room9_RightTrigger", rightBoundaryX - 0.25f, triggerY, triggerHeight, room10);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room9 スクロールトリガーを作成しました");
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
