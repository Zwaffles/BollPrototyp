using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SFX_Play_Bounce_Sound : MonoBehaviour
{
    public FMODUnity.EventReference BounceSound;
    private Rigidbody BallCollisionRb;

    
    private void OnCollisionEnter(Collision collision)
    {

            FMODUnity.RuntimeManager.PlayOneShot(BounceSound, transform.position);

    }


}
