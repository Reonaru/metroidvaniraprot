using UnityEngine;
using TMPro;
using System.Collections;

public class MessageFader : MonoBehaviour
{
    // Unityエディタから設定できるように public または [SerializeField] にする
    [SerializeField] private float fadeInDuration = 2.0f;  // フェードインにかかる時間
    [SerializeField] private float stayDuration = 3.0f;    // 表示を維持する時間
    [SerializeField] private float fadeOutDuration = 1.5f; // フェードアウトにかかる時間

    private TextMeshProUGUI textMesh;

    void Start()
    {
        // 1. TextMeshProUGUI コンポーネントを取得
        textMesh = GetComponent<TextMeshProUGUI>();

        // 2. 初期状態を完全に透明にする (alpha = 0)
        Color startColor = textMesh.color;
        startColor.a = 0f;
        textMesh.color = startColor;

        // 3. コルーチンを開始してフェード処理を実行
        StartCoroutine(FadeSequence());
    }

    public IEnumerator FadeSequence()
    {
        // === 1. フェードイン処理 ===
        yield return StartCoroutine(Fade(0f, 1f, fadeInDuration));

        // === 2. 維持処理 ===
        yield return new WaitForSeconds(stayDuration);

        // === 3. フェードアウト処理 ===
        yield return StartCoroutine(Fade(1f, 0f, fadeOutDuration));
        
        // （必要であれば、ここでテキストオブジェクトを非アクティブにする処理などを追加）
        // gameObject.SetActive(false);
    }

    /// <summary>
    /// テキストの Alpha 値を徐々に変化させるコルーチン
    /// </summary>
    /// <param name="startAlpha">開始時の透明度 (0f: 透明, 1f: 不透明)</param>
    /// <param name="endAlpha">終了時の透明度</param>
    /// <param name="duration">変化にかける時間</param>
    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        Color currentColor = textMesh.color;

        while (timer < duration)
        {
            // 時間の経過に応じて Alpha 値を Lerp で計算
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            
            // TextMeshPro の色を更新
            currentColor.a = newAlpha;
            textMesh.color = currentColor;

            timer += Time.deltaTime;
            yield return null; // 1フレーム待つ
        }

        // 最終的な値を確実に設定（ Lerp の誤差対策）
        currentColor.a = endAlpha;
        textMesh.color = currentColor;
    }
}