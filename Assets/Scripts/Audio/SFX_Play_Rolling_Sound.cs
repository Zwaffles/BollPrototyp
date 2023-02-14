using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SFX_Play_Rolling_Sound : MonoBehaviour
{

    //FMOD
    private FMODUnity.StudioEventEmitter emitter;

    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    private float currentSpeed;

    private Rigidbody ballRb;

    [SerializeField]
    private float minPitch;
    [SerializeField]
    private float maxPitch;
    private float pitchFromBall;

    private int currentCollisions = 0;

    void Start()
    {

        ballRb = GetComponent<Rigidbody>();

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

    }

    void Update()
    {

        EngineSound();

    }

    void EngineSound()
    {

        currentSpeed = ballRb.velocity.magnitude;
        pitchFromBall = ballRb.velocity.magnitude / 50f;

        float pitch, volume;
        
        if (currentSpeed < minSpeed) 
        {

            pitch = minPitch;
            volume = 0f;

        } 
        else
        {

            pitch = minPitch + pitchFromBall;
            volume = 1f;

        }

        emitter.SetParameter("Pitch", pitch);
        emitter.EventInstance.setVolume(currentCollisions > 0 ? volume : 0f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        currentCollisions++;
    }

    private void OnCollisionExit(Collision collision)
    {
        currentCollisions--;
    }

}
