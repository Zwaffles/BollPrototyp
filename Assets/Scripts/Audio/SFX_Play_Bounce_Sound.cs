using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/*
 *  Plays the bouncing SFX
 *  Original script by Ludwig, modified by Johan
 */

public class SFX_Play_Bounce_Sound : MonoBehaviour
{
    //Sound Reference
    public FMODUnity.EventReference BounceSound;
    //public FMODUnity.EventReference RollingSound;

    //Rigidbody
    private Rigidbody BallCollisionRb;

    //FMOD
    private FMOD.Studio.EventInstance instance;
    //public FMODUnity.EventReference RollingSound; //fmodEvent;

    private FMOD.Studio.PARAMETER_ID pitchParameterId;

    private int currentCollisions = 0;

    [SerializeField]
    [Range(0f, 1f)]
    private float eqGlobal;

    [SerializeField]
    [Range(0f, 1f)]
    private float reverb, delay;

    [SerializeField]
    [Range(0f, 5000f)]
    private float delayTime;

    [SerializeField]
    [Range(-12f, 12f)]
    private float pitch;



    //On Event Start
    void Start()
    {

        /*
         * I don't know what most of this does, but I'm too afraid to touch it RN
         */

        instance = FMODUnity.RuntimeManager.CreateInstance(BounceSound); //(fmodEvent);

        FMOD.Studio.EventDescription pitchEventDescription;
        instance.getDescription(out pitchEventDescription);

        FMOD.Studio.PARAMETER_DESCRIPTION pitchParameterDescription;
        pitchEventDescription.getParameterDescriptionByName("Pitch", out pitchParameterDescription);

        // What does this line do?
        pitchParameterId = pitchParameterDescription.id;
        instance.start();

        //FMOD
        instance.setParameterByName("Pitch", pitch);
        instance.setParameterByName("Delay", delay);
        instance.setParameterByName("Delay Time", delayTime);
        instance.setParameterByName("Reverb", reverb);

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("EQ Global", eqGlobal);

    }


    //On Event Colliding With Collision
    private void OnCollisionEnter(Collision collision)
    {

        if (currentCollisions++ != 0) return;
        FMODUnity.RuntimeManager.PlayOneShot(BounceSound, transform.position);

    }

    private void OnCollisionExit(Collision collision)
    {
        currentCollisions--;
    }

}
