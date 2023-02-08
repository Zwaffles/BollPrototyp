using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Player_Bounce_Sound : MonoBehaviour
{
    public FMODUnity.EventReference BounceSound;
    private Rigidbody BallCollisionRb;

    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(BounceSound, transform.position);
    }
    
    private void OnCollisionEnter(Collision collision)
    {

            FMODUnity.RuntimeManager.PlayOneShot(BounceSound, transform.position);

    }


}
