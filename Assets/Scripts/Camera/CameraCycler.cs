using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(InputManager))]
public class CameraCycler : MonoBehaviour
{

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera[] cameras;
    private int currentCameraPointer = 0;

    private const bool ALWAYS_FALSE = false;

    private InputManager inputs;

    void Start()
    {
        inputs = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (inputs.getCameraInput())
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
