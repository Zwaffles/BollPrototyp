using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SFX_Play_Rolling_Sound : MonoBehaviour
{

    public FMODUnity.EventReference RollingAudio;

    public float minSpeed;
    public float maxSpeed;
    private float currentSpeed;

    private Rigidbody ballRb;
    private AudioSource ballAudio;

    public float minPitch;
    public float maxPitch;
    private float pitchFromBall;

    void Start()
    {

        ballAudio = GetComponent<AudioSource>();
        ballRb = GetComponent<Rigidbody>();

    }

    void Update()
    {

        EngineSound();

    }

    void EngineSound()
    {

        currentSpeed = ballRb.velocity.magnitude;
        pitchFromBall = ballRb.velocity.magnitude / 50f;
        
        if (currentSpeed < minSpeed) 
        {

            ballAudio.volume = 1;
            ballAudio.pitch = minPitch;

        }

        if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {

            ballAudio.volume = 1;
            ballAudio.pitch = minPitch + pitchFromBall;

        }

        if (currentSpeed > maxSpeed)
        {

            ballAudio.volume = 1;
            ballAudio.pitch = maxPitch;

        }

    }
}
