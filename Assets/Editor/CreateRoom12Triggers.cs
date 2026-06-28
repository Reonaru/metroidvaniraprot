using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom12Triggers
{
    [MenuItem("Tools/Create Room12 Triggers")]
    public static void CreateTriggers()
    {
        // Room12 の境界線
        float leftBoundaryX = 398f;
        float triggerY = -10f;
        float triggerHeight = 10f;

        // RoomData を読み込む
        RoomData room11 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room11.asset");

        // 既存トリガーを削除
        GameObject existingLeftTrigger = GameObject.Find("Room12_LeftTrigger");
        if (existingLeftTrigger != null)
        {
            Object.DestroyImmediate(existingLeftTrigger);
            Debug.Log("既存の Room12_LeftTrigger を削除しました");
        }

        // 左端トリガー（Room12→Room11）
        CreateTrigger("Room12_LeftTrigger", leftBoundaryX + 0.25f, triggerY, triggerHeight, room11);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room12 スクロールトリガーを作成しました");
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
