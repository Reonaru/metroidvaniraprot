using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CreateRoom11Shutter
{
    [MenuItem("Tools/Create Room11 Shutter")]
    public static void CreateShutter()
    {
        // 既存のシャッターを削除
        GameObject existingShutter = GameObject.Find("Room11_Shutter");
        if (existingShutter != null)
        {
            Object.DestroyImmediate(existingShutter);
            Debug.Log("既存の Room11_Shutter を削除しました");
        }

        GameObject shutterObj = new GameObject("Room11_Shutter");
        shutterObj.transform.position = new Vector3(398, 0, 0);

        // Rigidbody2D
        Rigidbody2D rb = shutterObj.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;

        // BoxCollider2D（物理・プレイヤー衝突）
        BoxCollider2D colliderPhysics = shutterObj.AddComponent<BoxCollider2D>();
        colliderPhysics.size = new Vector2(1, 2);
        colliderPhysics.isTrigger = false;

        // BoxCollider2D（トリガー・弾検知）
        BoxCollider2D colliderTrigger = shutterObj.AddComponent<BoxCollider2D>();
        colliderTrigger.size = new Vector2(1, 2);
        colliderTrigger.isTrigger = true;

        // SpriteRenderer
        SpriteRenderer spriteRenderer = shutterObj.AddComponent<SpriteRenderer>();
        spriteRenderer.color = Color.gray;

        // Shutter11スクリプト
        shutterObj.AddComponent<Shutter11>();

        shutterObj.tag = "shatter";

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Room11シャッターを作成しました（X=398, 敵全滅後に破壊可能）");
    }
}
