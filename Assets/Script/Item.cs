using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemValue = 1;
    private ItemCounter itemCounter;

    void Start()
    {
        itemCounter = FindObjectOfType<ItemCounter>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemCounter != null)
            {
                itemCounter.AddItems(itemValue);
            }
            Debug.Log($"アイテム取得: +{itemValue}");
            Destroy(gameObject);
        }
    }
}
