using UnityEngine;
using System.Collections; // コルーチンを使うために必要

// 背景のライトを制御し、イベントを購読するクラス
public class BackgroundLightController : MonoBehaviour
{
    [Header("点滅させるライト")]
    public Light backgroundLight; // URPの2Dライトを使用

    [Header("点滅設定")]
    public float maxIntensity = 3.0f; // 稲妻の最大強度
    public float flashDuration = 0.5f; // 点滅にかける時間 (短いほど速い)

    void Start()
    {
        // 💡 ゲーム開始時、ライトの強度を必ず 0 に設定する
        // これで、Trigger に入るまで背景のシルエットは見えない状態が保証される
        if (backgroundLight != null)
        {
            backgroundLight.intensity = 0f;
        }
    }


    void OnEnable()
    {
        // 💡 イベントの購読（Triggerから通知を受け取るための登録）
        BackgroundEventTrigger.OnFlashEncounter += StartFlashSequence;
    }

    void OnDisable()
    {
        // 💡 イベントの購読解除（オブジェクトが非アクティブになる時に忘れずに）
        BackgroundEventTrigger.OnFlashEncounter -= StartFlashSequence;
    }

    // イベントを受け取ったときに実行される関数
    private void StartFlashSequence()
    {
        if (backgroundLight != null)
        {
            StartCoroutine(FlashLight());
        }
    }

    // 稲妻のような点滅演出を実行するコルーチン
    private IEnumerator FlashLight()
    {
        // 演出開始前の元の強度を保存
        float originalIntensity = backgroundLight.intensity;
        
        // 1. 瞬間的に強く光らせる
        backgroundLight.intensity = maxIntensity;
        
        // 2. 短時間待機
        yield return new WaitForSeconds(10.0f); 
        
        // 3. すぐに暗くする（消える瞬間）
        backgroundLight.intensity = 0f;
        
        // 4. 再度短時間だけ光らせて残像感を出す
        yield return new WaitForSeconds(0.1f);
        
        // 5. 完全に消す
        backgroundLight.intensity = 0f;
        
        // 演出完了後、元の強度に戻す（今回の演出では完全に消す）
        // backgroundLight.intensity = originalIntensity;
    }
}