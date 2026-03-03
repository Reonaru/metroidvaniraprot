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

            Destroy(gameObject);
            Debug.Log("敵に命中");



        }

        if (other.CompareTag("shatter"))
        {

            Destroy(gameObject);
            Debug.Log("shatter");



        }

    }
}
