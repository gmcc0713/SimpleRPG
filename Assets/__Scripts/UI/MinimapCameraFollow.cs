using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [SerializeField] private bool x, y, z;
    [SerializeField] private Transform target;

    void Start()
    {
        if(target == null)
            target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if(!target)
        {
            return;
        }
        transform.position = new Vector3(x ? target.position.x : transform.position.x, y ? target.position.y : transform.position.y, z ? target.position.z : transform.position.z);
    }
}
