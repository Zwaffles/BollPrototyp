using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(PlayerController))]
public class BounceController : MonoBehaviour
{

    [SerializeField, Header("Bounce Controls"), Tooltip("How strong the impulse needs to be for a bounce to trigger"), Range(0f, 20f)]
    private float impulseThreshold = 1f;
    [SerializeField, Tooltip("How much bias the bounce should have horizontally and vertically")]
    private Vector2 bounceBias = new Vector2(1f, 1f);
    [SerializeField, Tooltip("How weak the bounce should be. DO NOT USE THIS TO STRENGTHEN THE BOUNCE"), Range(0f, 1f)]
    private float bounceStrength = 0.5f;
    [SerializeField, Tooltip("How much the bounce should be offset by, allowing you to bounce over obstacles")]
    private Vector2 bounceOffset = new Vector2(0f, 0f);
    private Vector3 internalBounceOffset;

    private bool canBounce = false;

    private Rigidbody rb;
    private PlayerController playerController;

    private void OnCollisionEnter(Collision collision)
    {

        // This is where the bounce happens

        //Debug.DrawLine(transform.position, transform.position + collision.impulse * 0.5f, Color.red, 1f);

        if (!canBounce) return;
        
        if (collision.impulse.magnitude < impulseThreshold) return;

        //GameManager.instance.audioManager.PlaySfx("Rollin", 0.1f);

        rb.AddForce((collision.impulse + internalBounceOffset) * bounceBias * bounceStrength, ForceMode.Impulse);

        GameManager.instance.achievementManager.AddStat(Stat.Bounces, 1);

        canBounce = false;

    }

    private void Update()
    {
        if (!playerController.isGrounded) canBounce = true;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        internalBounceOffset = new Vector3(bounceOffset.x, bounceOffset.y, 0f);
    }

}
