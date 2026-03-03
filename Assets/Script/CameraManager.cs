using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target;
    Vector3 fixedPos;


    // Start is called before the first frame update
    void Start()
    {
      //   fixedPos = new Vector3(0,0,-50);
      //   Camera.main.gameObject.transform.position = fixedPos;   
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 cameraPos = target.transform.position;

      if(target.transform.position.x < 0)
      {
        cameraPos.x = 0;
      }
//
      if(target.transform.position.y < 0 || target.transform.position.y > 0)
      {
          cameraPos.y = 0;
      }
//
      if(target.transform.position.y > 0)
        {
            cameraPos.y = target.transform.position.y;
        }
//
      cameraPos.z = -10;
      Camera.main.gameObject.transform.position = cameraPos;

    }


}
