using UnityEngine;
using Cinemachine;


//Eventually, everything related to the camera should be handled by a manager.
//This means that, among other things, a reference to the InputReader will be unnecessary since it will be contained within the GameManager singleton
public class CameraCycler : MonoBehaviour
{
    [SerializeField]
    private InputReader input;

    private CinemachineVirtualCamera[] cameras;
    
    private int currentCameraPointer = 0;

    private bool shouldCycle = false;

    void Start()
    {
        input.CameraCycleEvent += HandleCameraCycle;

        cameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        System.Array.Sort<CinemachineVirtualCamera>(cameras,
            (a, b) => -b.name.CompareTo(a.name)
            );
    }

    // Update is called once per frame
    void Update()
    {
        
        if (shouldCycle)
        {
            shouldCycle = false;

            cameras[currentCameraPointer].enabled = false;

            currentCameraPointer++;

            if (currentCameraPointer >= cameras.Length)
            {
                currentCameraPointer = 0;
            }

            cameras[currentCameraPointer].enabled = true;
        }
    }

    private void HandleCameraCycle()
    {
        shouldCycle = true;
    }
}
