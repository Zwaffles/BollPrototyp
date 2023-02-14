using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(InputManager))]
public class CameraCycler : MonoBehaviour
{

    [SerializeField, Header("Cameras"), Tooltip("A list of which cameras to use. If left empty the script will automatically find all the cameras in the scene.")]
    private Cinemachine.CinemachineVirtualCamera[] cameras;
    
    private int currentCameraPointer = 0;

    private InputManager inputs;

    void Start()
    {
        inputs = GetComponent<InputManager>();


        // Automatically find cameras in scene
        if (cameras.Length == 0)
        {
            cameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        }

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
