using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObj : MonoBehaviour
{
    public GameObject Object;
        
    void Update()
    {
        if (Object != null)
        {            
            Camera.main.transform.position = new Vector3(Object.transform.position.x, 1, -10);
        }
    }
}
