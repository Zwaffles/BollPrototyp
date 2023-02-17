using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraAutoDetect : MonoBehaviour
{

    private CinemachineVirtualCamera vCam;
    
    // Start is called before the first frame update
    void Start()
    {

        vCam = GetComponent<CinemachineVirtualCamera>();

        vCam.Follow = GameObject.Find("Ball").transform;
        vCam.LookAt = GameObject.Find("Ball").transform;

    }

}
