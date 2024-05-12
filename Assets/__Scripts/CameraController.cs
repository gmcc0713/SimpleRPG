using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineFreeLook m_cam;
    void Start()
    {
        m_cam = GetComponent<CinemachineFreeLook>();
        m_cam.Follow = GameObject.Find("Player").transform;
        m_cam.LookAt = GameObject.Find("Player").transform;
    }

    // Update is called once per frame

}
