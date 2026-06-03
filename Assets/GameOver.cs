using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;

    public void Show()
    {
        gameOverUI.SetActive(true);
        gameOverUI.GetComponent<GameOverUI>().ShowGameOver();
    }
}