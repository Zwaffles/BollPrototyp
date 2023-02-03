using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("Movement Values")] 
    private float initialSpeed = 20.0f;
    [SerializeField] 
    private float speedIncrement = 5.0f;
    [SerializeField] 
    private float slopeForce = 5.0f;

    private float speed;

    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = initialSpeed;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(vertical, 0, 0);

        RaycastHit hit;
        if(Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hit, controller.height / 2 * transform.localScale.y, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if(slopeAngle > controller.slopeLimit)
            {
                direction = Vector3.Cross(hit.normal, direction);
                direction *= slopeForce / slopeAngle;
            }
            else
            {
                speed = Mathf.Min(speed + speedIncrement * Time.deltaTime, initialSpeed + speedIncrement);
            }
        }
        direction = direction.normalized * Mathf.Min(direction.magnitude, speed);

        controller.SimpleMove(direction);
    }

}
