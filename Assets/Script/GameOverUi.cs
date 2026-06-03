using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI設定")]
    public GameObject gameOverPanel;
    public CanvasGroup canvasGroup;    // GameOverPanelのCanvasGroup

    [Header("フェード設定")]
    public float fadeDuration = 1.5f;  // フェードにかける秒数
    public float waitBeforeFade = 0.5f; // 死亡後フェード開始までの待機時間

    private PlayerCollition playerCollition;

    void Start()
    {
        playerCollition = FindObjectOfType<PlayerCollition>();

        // 最初は完全に非表示
        gameOverPanel.SetActive(false);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;      // ボタン操作を無効化
        canvasGroup.blocksRaycasts = false;    // クリック判定も無効化
    }

    // PlayerCollitionから呼ぶ
    public void ShowGameOver()
    {
        Debug.Log("ゲームオーバー表示");
        StartCoroutine(FadeInGameOver());
    }

    IEnumerator FadeInGameOver()
    {
        // 少し待ってからフェード開始（死亡の瞬間をみせる）
        yield return new WaitForSecondsRealtime(waitBeforeFade);

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ゲーム一時停止

        // α値を0→1にアニメ
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // timeScale=0でも動く
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;    // フェード完了後にボタン有効化
        canvasGroup.blocksRaycasts = true;
    }

    // リトライボタンから呼ぶ
    public void OnRetryButton()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        playerCollition.RetryGame();
    }
}