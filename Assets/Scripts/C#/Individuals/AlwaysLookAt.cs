using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAt : MonoBehaviour
{
    [SerializeField]
    Transform lookAtObject;


    void Update()
    {
        if (lookAtObject)
            transform.LookAt(lookAtObject);
    }
}
