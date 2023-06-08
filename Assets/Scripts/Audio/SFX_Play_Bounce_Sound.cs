using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Plays the bouncing SFX
 *  Original script by Ludwig, modified by Johan
 */

public class SFX_Play_Bounce_Sound : MonoBehaviour
{
    //Rigidbody
    private Rigidbody ballCollisionRb;

    private int currentCollisions = 0;

    [SerializeField]
    [Range(-12f, 12f)]
    private float pitch = 1f;

    [SerializeField, Tooltip("How strong the impulse from a collision has to be to trigger the SFX")]
    [Range(0f, 20f)]
    private float minimumImpulse = 2f;

    //On Event Start
    void Start()
    {

        ballCollisionRb = GetComponent<Rigidbody>();

    }


    //On Event Colliding With Collision
    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Don't get me wrong, I love you");

        //if (currentCollisions++ != 0) return;

        Debug.Log("But does that mean I have to meet your father?");

        if (collision.impulse.magnitude < minimumImpulse) return;

        Debug.Log("When we are older, you'll understand");

        GameManager.instance.audioManager.PlaySfx("airhorn", pitch);

        Debug.Log("No, I don't think life is quite that simple");

    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.LogWarning("Estuans interius - Ira vehementi");
        currentCollisions--;
        Debug.LogWarning("Estuans interius - Ira vehementi");
        Debug.LogError("Sephiroth");
        Debug.LogError("Sephiroth");
    }

}
