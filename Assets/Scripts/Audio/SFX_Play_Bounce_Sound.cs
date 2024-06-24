using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Plays the bouncing SFX
 *  Original script by Ludwig, modified by Johan
 */

public class SFX_Play_Bounce_Sound : MonoBehaviour
{

    [SerializeField]
    [Range(-12f, 12f)]
    private float pitch = 1f;

    [SerializeField, Tooltip("How strong the impulse from a collision has to be to trigger the SFX")]
    [Range(0f, 20f)]
    private float minimumImpulse = 2f;

    private float lastBounceTime;
    private const float MIN_TIME_BETWEEN_BOUNCES = 0.3f;

    private PlayerController playerController;

    private bool wasGrounded = false;

    //On Event Start
    void Start()
    {

        lastBounceTime = Time.time;
        playerController = GetComponent<PlayerController>();

    }


    //On Event Colliding With Collision
    private void OnCollisionEnter(Collision collision)
    {

        if (wasGrounded) return;

        wasGrounded = true;

        if (collision.impulse.magnitude < minimumImpulse) return;

        if (Time.time < lastBounceTime + MIN_TIME_BETWEEN_BOUNCES) return;

        GameManager.instance.audioManager.PlaySfx(string.Format("BallBounce{0}", Mathf.FloorToInt(Random.Range(1f, 4.9f))), pitch); //"Basket_Bounce_1-SFX"
        lastBounceTime = Time.time;

    }

    private void Update()
    {
        if (!playerController.isGrounded)
        {

            wasGrounded = false;

        }

    }

}
