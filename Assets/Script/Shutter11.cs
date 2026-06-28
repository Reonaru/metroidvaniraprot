using UnityEngine;

public class Shutter11 : MonoBehaviour
{
    private bool canBeDestroyed = false;

    void Update()
    {
        if (!canBeDestroyed && Gmanager.Instance.GetFlag("Room11_Clear"))
        {
            canBeDestroyed = true;
            Debug.Log("シャッターが破壊可能になりました");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBeDestroyed)
            return;

        if (other.CompareTag("bullet"))
        {
            Destroy(gameObject);
            Debug.Log("Room11シャッターが破壊されました");
        }
    }
}
