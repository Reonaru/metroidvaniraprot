using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockActivator : MonoBehaviour
{
    public string targetFlagName; 
    public GameObject blockObject; // 💡 出現させたいブロック（InactiveでOK）をここにドラッグ
    public GameObject blockObject2;

    void Update()
    {
        // ブロックがまだ非アクティブ、かつフラグが立った時だけ実行
        if (!blockObject.activeSelf && Gmanager.Instance.GetFlag(targetFlagName))
        {
            blockObject.SetActive(true);
            blockObject2.SetActive(true);
            Debug.Log($"{blockObject.name} を外部から起動しました");
        }
    }
}