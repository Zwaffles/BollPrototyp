using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset;

    [SerializeField, Header("Camera Follow Values"), Tooltip("The speed at which the camera moves towards the desired position")]
    private float smoothSpeed = 0.2f;
    [SerializeField, Tooltip("How much the velocity of the rigidbody is multiplied with to get the xOffset of the camera follow")]
    private float xOffsetFactor = 0.3f;
    [SerializeField, Tooltip("The minimum amount of velocity needed for the camera to follow the player")]
    private float followThreshold = 0.3f;
    [SerializeField, Tooltip("The maximum amount of xOffset for the camera")]
    private float maxXOffset = 1f;
    [SerializeField, Tooltip("The size of the deadzone around the target's position")]
    private float deadzoneRadius = 2f;

    [SerializeField, Header("Camera Return Values"), Tooltip("The time it takes for the camera to return to center on target")]
    private float smoothReturnTime = 2f;
    [SerializeField, Tooltip("The amount that the return speed is multiplied by based on the distance the camera is from the player. Smaller values result in a slower movement")]
    private float returnSpeedFactor = 0.4f;

    private Vector3 _desiredPosition;
    private Vector3 _smoothedPosition;

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position;
        Vector3 xOffset = new Vector3(targetPosition.x * xOffsetFactor, 0, 0);
        Vector3 clampedXOffset = Vector3.Max(Vector3.Min(xOffset, new Vector3(maxXOffset, 0, 0)), new Vector3(-maxXOffset, 0, 0));
        _desiredPosition = target.position + offset + clampedXOffset;

        if(Vector3.Distance(target.position, _desiredPosition) < deadzoneRadius)
        {
            _smoothedPosition = target.position;
        }

        else
        {
            _smoothedPosition = Vector3.MoveTowards(transform.position, _desiredPosition, smoothSpeed * Time.deltaTime);

            if (targetPosition.magnitude < followThreshold)
            {
                Vector3 centerOfDeadzone = transform.position + offset;
                float distanceFromCenter = Vector3.Distance(target.position, centerOfDeadzone);
                float smoothReturnSpeed = Mathf.Sqrt(distanceFromCenter) * returnSpeedFactor;
                _smoothedPosition.x = Mathf.SmoothDamp(_smoothedPosition.x, target.position.x + offset.x, ref smoothReturnSpeed, smoothReturnTime);
            }

            transform.position = _smoothedPosition;
        }
    }
}
