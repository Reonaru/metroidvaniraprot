using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shatter : MonoBehaviour
{
    public string targetFlagName; // インスペクターから "Room1_Clear" とか入れる

    void Update()
    {
    bool isClear = Gmanager.Instance.GetFlag(targetFlagName);
    
    // 💡 ログを出して、今何を読み取っているか強制的に表示させる

        // 💡 Gmanagerに「俺が開くべきフラグは true になったか？」と聞き続ける
        if (isClear)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        Debug.Log($"{gameObject.name} が開きました！");
        // 最もシンプルな「扉が開く」処理
        gameObject.SetActive(false); 
    }
}