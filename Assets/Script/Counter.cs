using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{

    private int second;

    private float countTime = 60;

    public Text timeCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countTime -= Time.deltaTime;
        second = (int)countTime;
       //(int)countTimeでint型に変換して表示させる。
			timeCount.text = second.ToString();
    }
}




