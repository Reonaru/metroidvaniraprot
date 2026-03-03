using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Playersc playerScript;
    SpriteRenderer playerRenderer;
    // Start is called before the first frame update
    void Start()
    {
     CheckComponents();
     SetupCollisionDetector();   
    }

    // Update is called once per frame
    void CheckComponents()
{
    // 1. 親オブジェクトの取得確認
    Debug.Log($"親オブジェクト: {transform.parent?.name}");
    
    // 2. Playerscスクリプトの取得確認
    playerScript = GetComponentInParent<Playersc>();
    if (playerScript != null)
    {
        Debug.Log("✓ Playerscスクリプト取得成功");
    }
    else
    {
        Debug.LogError("✗ Playerscスクリプトが見つかりません！");
    }
    
    // 3. SpriteRendererの取得確認
    playerRenderer = GetComponentInParent<SpriteRenderer>();
    if (playerRenderer != null)
    {
        Debug.Log("✓ SpriteRenderer取得成功");
    }
    else
    {
        Debug.LogError("✗ SpriteRendererが見つかりません！");
    }
}
void SetupCollisionDetector() 
{
    // このオブジェクトにCircleCollider2Dを追加（ダメージ判定用）
    CircleCollider2D damageCollider = gameObject.AddComponent<CircleCollider2D>(); 
    damageCollider.radius = 0.6f; 
    damageCollider.isTrigger = false; // Triggerとして設定
    Debug.Log("当たり判定システム初期化完了"); 
}
}
