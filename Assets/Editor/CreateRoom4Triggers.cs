using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom4Triggers
{
    [MenuItem("Tools/Create Room4 Triggers")]
    public static void CreateTriggers()
    {
        // 既存の Room4 Trigger を削除
        GameObject existingRightTrigger = GameObject.Find("Room4_RightTrigger");
        if (existingRightTrigger != null)
        {
            Object.DestroyImmediate(existingRightTrigger);
            Debug.Log("既存の Room4_RightTrigger を削除しました");
        }

        // Room4 の境界線
        float boundaryX = 220f;
        float triggerY = 0f;
        float triggerHeight = 10f;

        // RoomData を読み込む
        RoomData room7 = AssetDatabase.LoadAssetAtPath<RoomData>("Assets/scriptable/room7.asset");

        // 右端トリガー（Room4→Room7）- 境界線より0.25手前
        CreateTrigger("Room4_RightTrigger", boundaryX - 0.25f, triggerY, triggerHeight, room7);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room4 スクロールトリガーを作成しました");
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
