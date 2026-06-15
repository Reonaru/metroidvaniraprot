using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public float speed = 10f;
    public float lifeTime = 3f;

private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            // 敵にダメージとノックバックを与える
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(3, transform.position);
                Debug.Log("敵に命中 - ダメージ＆ノックバック");
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("shatter"))
        {
            // シャッターを揺らす
            DoorController door = other.GetComponent<DoorController>();
            if (door != null)
            {
                door.Shake();
            }

            Destroy(gameObject);
            Debug.Log("shatter");
        }
    }
}
