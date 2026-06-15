using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTrigger : MonoBehaviour
{
    public RoomData targetRoom;
    private RoomManager roomManager;

    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && targetRoom != null)
        {
            roomManager.StartScroll(targetRoom);
        }
    }
}
