using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public CameraManager1 cameraManager;
    public GameObject messageLocation; // メッセージの表示位置など
    public GameObject messageFaderObject; 
    private MessageFader messageFader;

    void Start()
    {

        // 1. 演出に必要な MessageFader を、設定されたオブジェクトから取得する
        // ✅ 修正: 自身からではなく、設定された GameObject から GetComponent する
        if (messageFaderObject != null)
        {
            // GetComponentInChildren を使う場合は、そのコンポーネントが子オブジェクトに付いている場合です
            messageFader = messageFaderObject.GetComponent<MessageFader>();
            
            if (messageFader == null)
            {
                Debug.LogError("messageFaderObject に MessageFader コンポーネントが付いていません！");
                return;
            }
        }

        // 演出開始
        StartCoroutine(PlayIntroSequence());

    }

    private IEnumerator PlayIntroSequence()
    {
        // 演出の目標位置を設定
        Vector3 zoomInPos = messageLocation.transform.position;
        zoomInPos.z = -10; // カメラのZ軸を合わせる

        // 1. メッセージの位置にズームイン (2秒間)
        yield return StartCoroutine(cameraManager.DoSpecialMove(zoomInPos, 2.0f, 2.0f));

        // 2. メッセージ表示など (ここでメッセージのフェードイン/アウトのコルーチンを呼び出す)
     //    yield return StartCoroutine(messageFader.FadeSequence());

        // 3. プレイヤーの位置に戻ってゲームプレイ開始画角に戻す (2秒間)
        Vector3 gameStartPos = cameraManager.target.transform.position;
        gameStartPos.z = -10;
        yield return StartCoroutine(cameraManager.DoSpecialMove(gameStartPos, 5.0f, 2.0f)); // 5.0f は通常時のカメラサイズ

        Debug.Log("ゲームプレイ開始！");
    }
}