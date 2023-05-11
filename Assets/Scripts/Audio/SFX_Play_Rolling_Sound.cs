using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Plays the rolling SFX
 *  Original script by Ludwig, modified by Johan
 */
[RequireComponent(typeof(PlayerController))]
public class SFX_Play_Rolling_Sound : MonoBehaviour
{

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

    private PlayerController playerController;

    void OnEnable()
    {

        ballRb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();

        GameManager.instance.audioManager.PlayLoopingSfx("Rollin", isRunning, EngineSound);

    }

    float EngineSound()
    {

        currentSpeed = Mathf.Min(ballRb.angularVelocity.magnitude, maxSpeed);

        pitchFromBall = currentSpeed / 50f;

        float pitch;
        
        if (currentSpeed < minSpeed) 
        {

            pitch = minPitch;

        } 
        else
        {

            pitch = Mathf.Min(maxPitch, minPitch + pitchFromBall);

        }

        return pitch;

    }

    bool isRunning()
    {
        return playerController.isGrounded;
    }

}
