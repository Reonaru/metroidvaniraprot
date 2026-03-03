using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flag_open : MonoBehaviour
{

    public Text jpflag;

    private Playersc ps;

    private string ju;
    
    // Start is called before the first frame update
    void Start()
    {
        ps = FindObjectOfType<Playersc>();
    }

    // Update is called once per frame
    void Update()
    {
        ju = ps.jumpInput.ToString();
        jpflag.text = ju.ToString();
        Debug.Log($"jumpflag:{ju}");
    }
}
