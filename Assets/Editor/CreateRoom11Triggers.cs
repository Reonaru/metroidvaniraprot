using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom11Triggers
{
    [MenuItem("Tools/Create Room11 Triggers")]
    public static void CreateTriggers()
    {
        // Room11 の境界線
        float leftBoundaryX = 358f;
        float rightBoundaryX = 398f;
        float triggerY = -10f;
        float triggerHeight = 10f;

        // RoomData を読み込む
        RoomData room10 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room10.asset");
        RoomData room12 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room12.asset");

        // 既存トリガーを削除
        GameObject existingLeftTrigger = GameObject.Find("Room11_LeftTrigger");
        if (existingLeftTrigger != null)
        {
            Object.DestroyImmediate(existingLeftTrigger);
            Debug.Log("既存の Room11_LeftTrigger を削除しました");
        }

        GameObject existingRightTrigger = GameObject.Find("Room11_RightTrigger");
        if (existingRightTrigger != null)
        {
            Object.DestroyImmediate(existingRightTrigger);
            Debug.Log("既存の Room11_RightTrigger を削除しました");
        }

        // 左端トリガー（Room11→Room10）
        CreateTrigger("Room11_LeftTrigger", leftBoundaryX + 0.25f, triggerY, triggerHeight, room10);

        // 右端トリガー（Room11→Room12）
        if (room12 != null)
        {
            CreateTrigger("Room11_RightTrigger", rightBoundaryX - 0.25f, triggerY, triggerHeight, room12);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room11 スクロールトリガーを作成しました");
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
