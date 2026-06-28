using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private PlayerCollition playerCollition;
    private RectTransform barRectTransform;
    private float maxHealth = 100f;
    private float maxWidth;

    void Start()
    {
        playerCollition = FindObjectOfType<PlayerCollition>();
        barRectTransform = GetComponent<RectTransform>();

        if (barRectTransform != null)
        {
            maxWidth = barRectTransform.sizeDelta.x;
        }

        if (playerCollition != null)
        {
            maxHealth = playerCollition.health;
        }
    }

    void Update()
    {
        if (playerCollition != null && barRectTransform != null)
        {
            float healthRatio = playerCollition.health / maxHealth;
            float currentWidth = maxWidth * Mathf.Clamp01(healthRatio);
            barRectTransform.offsetMax = new Vector2(currentWidth, 0);
        }
    }
}
