using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/*
 *  Plays the rolling SFX
 *  Original script by Ludwig, modified by Johan
 */

[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class SFX_Play_Rolling_Sound : MonoBehaviour
{

    //FMOD
    private FMODUnity.StudioEventEmitter emitter;

    [SerializeField, Header("Sound Controls"), Tooltip("The minimum speed to play the SFX")]
    private float minSpeed = 1f;
    [SerializeField, Tooltip("The maximum speed that affects the pitch of the SFX")]
    private float maxSpeed = 10f;
    private float currentSpeed;

    private Rigidbody ballRb;

    [SerializeField, Tooltip("The minimum pitch to play the SFX at, 0 = default")]
    private float minPitch = 0f;
    [SerializeField, Tooltip("The maximum pitch to play the SFX at")]
    private float maxPitch = 0.4f;
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

        currentSpeed = Mathf.Min(ballRb.velocity.magnitude, maxSpeed);

        pitchFromBall = currentSpeed / 50f;

        float pitch, volume;
        
        if (currentSpeed < minSpeed) 
        {

            pitch = minPitch;
            volume = 0f;

        } 
        else
        {

            pitch = Mathf.Min(maxPitch, minPitch + pitchFromBall);
            volume = 1f;

        }

        // Set pitch and volume (if the ball is rolling on something)
        emitter.SetParameter("Pitch", pitch);
        emitter.EventInstance.setVolume(currentCollisions > 0 ? volume : 0f);

    }

    // Keep track of how many surfaces the ball is touching
    private void OnCollisionEnter(Collision collision)
    {
        currentCollisions++;
    }

    private void OnCollisionExit(Collision collision)
    {
        currentCollisions--;
    }

}
