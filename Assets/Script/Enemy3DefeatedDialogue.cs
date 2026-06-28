using UnityEngine;
using UnityEngine.UI;

public class Enemy3DefeatedDialogue : MonoBehaviour
{
    private bool dialogueShown = false;

    void Update()
    {
        if (!dialogueShown && Gmanager.Instance.GetFlag("Room3_Enemy_Defeated"))
        {
            ShowDialogue("「やった！敵を倒した！」");
            dialogueShown = true;
        }
    }

    void ShowDialogue(string text)
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            Debug.LogError("Canvas が見つかりません");
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player が見つかりません");
            return;
        }

        // 既存のダイアログを削除
        Transform existingDialogue = canvasObj.transform.Find("Room3_Defeated_Dialogue");
        if (existingDialogue != null)
        {
            Destroy(existingDialogue.gameObject);
        }

        // ダイアログ用のパネルを作成
        GameObject dialogueObj = new GameObject("Room3_Defeated_Dialogue");
        dialogueObj.transform.SetParent(canvasObj.transform, false);

        RectTransform rectTransform = dialogueObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.sizeDelta = new Vector2(200, 60);

        // プレイヤーのすぐ上に表示
        Vector3 playerWorldPos = playerObj.transform.position + new Vector3(0, 2.5f, 0);
        rectTransform.position = playerWorldPos;

        // 背景パネル
        Image panelImage = dialogueObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);

        // テキスト
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(dialogueObj.transform, false);

        Text dialogueText = textObj.AddComponent<Text>();
        dialogueText.text = text;
        dialogueText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        dialogueText.fontSize = 24;
        dialogueText.fontStyle = FontStyle.Bold;
        dialogueText.color = Color.white;
        dialogueText.alignment = TextAnchor.MiddleCenter;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Debug.Log("敵撃破ダイアログを表示しました");

        // 3秒後に消える
        Destroy(dialogueObj, 3f);
    }
}
