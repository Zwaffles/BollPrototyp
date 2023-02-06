using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCycler : MonoBehaviour
{

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera[] cameras;
    private int currentCameraPointer = 0;

    private const bool ALWAYS_FALSE = false;

    // Update is called once per frame
    void Update()
    {
        
        if (ALWAYS_FALSE)
        {

            cameras[currentCameraPointer].enabled = false;

            currentCameraPointer++;

            if (currentCameraPointer >= cameras.Length)
            {
                currentCameraPointer = 0;
            }

            cameras[currentCameraPointer].enabled = true;

        }

    }
}
