using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roomidscri : MonoBehaviour
{

    private RoomManager roomManager;

    public Text label;
    // Start is called before the first frame update
    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        drawroomid();
        
    }


    void drawroomid(){
        int currentID = roomManager.currentRoomID;
        label.text = "現在の部屋:" + currentID;
    }
}
