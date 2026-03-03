using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerenable : MonoBehaviour
{

    private Playersc playersc;
    private PlayerCollition coli;

    // Start is called before the first frame update
    void Start()
    {
        playersc = FindObjectOfType<Playersc>();
        coli = FindObjectOfType<PlayerCollition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
                Debug.Log("testenable");
                playersc.enabled = true;
                coli.health += 20;
        }

    }
}
