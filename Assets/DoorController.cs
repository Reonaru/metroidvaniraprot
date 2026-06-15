using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private int bulletHitCount = 0;
    private int bulletHitThreshold = 5;
    private bool isOpened = false;

    void Update()
    {
        // 弾5発受けたら開く
        if (bulletHitCount >= bulletHitThreshold && !isOpened)
        {
            OpenDoor();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"何かが衝突した: {collision.gameObject.name}, タグ: {collision.tag}");

        if (collision.CompareTag("bullet"))
        {
            bulletHitCount++;
            Debug.Log($"ドアが弾を受けた！ カウント: {bulletHitCount}/{bulletHitThreshold}");

            if (bulletHitCount >= bulletHitThreshold)
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        isOpened = true;
        Debug.Log($"{gameObject.name} が開きました！");
        gameObject.SetActive(false);
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 originalPos = transform.position;
        float shakeAmount = 0.3f;
        float shakeDuration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * shakeAmount;
            yield return null;
        }

        transform.position = originalPos;
    }
}