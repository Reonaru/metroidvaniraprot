using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSenser : MonoBehaviour{
    public PlayerCollition master; // インスペクターでPlayer本体のPlayerCollitionをドラッグして入れる

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ground")) master.isground = true;
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("ground")) master.isground = false;
    }
}
