using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;

    [SerializeField, Header("Movement")]
    private float moveAcceleration = 100f;
    [SerializeField]
    private float currentMaxSpeed = 28f; // Renamed from moveSpeed
    [SerializeField]
    private float downSlopeSpeedMultiplier = 4;
    [SerializeField]
    private float maxDownSlopeSpeed = 80;
    [SerializeField]
    private float upSlopeSpeedMultiplier = 0.5f;
    [SerializeField]
    private float minUpSlopeSpeed = 10;
    [SerializeField, Header("Jump")]
    private bool hasJump = true;
    [SerializeField, HideInInspector]
    private float jumpHeight = 2f;

    // Variables for Ludwig's ramping speed suggestion - Johan
    [SerializeField, Header("Speed Ramping"), Tooltip("How quickly the speed increases when you're BELOW the regular max speed.")]
    private float fastSpeedRampUpFactor = 2f;
    [SerializeField, Tooltip("How quickly the speed increases when you're ABOVE the regular max speed.")]
    private float slowSpeedRampUpFactor = 0.15f;
    [SerializeField, Tooltip("How much above the regular max speed you can go.")]
    private float maxOverSpeed = 50f;
    private float currentOverSpeed = 0f;
    [SerializeField, Tooltip("Max-speed the ball should lerp towards")]
    private float targetMaxSpeed = 28f;

    [SerializeField, Header("Gravity"), Tooltip("Regular gravity when on ground")]
    private float standardGravity = 9.8f; // There's a risk that this is different from the Physics default...
    private float temporaryGravity = 9.8f;
    private float previousGravity = 9.8f;
    public float Gravity
    {
        get => previousGravity;
    }
    [SerializeField, Tooltip("How quickly the gravity increases when you're in air")]
    private float gravityIncreaseFactor = 20f;
    [SerializeField, Tooltip("Maximum strength of the gravity")]
    private float maximumGravity = 40f;

    private Rigidbody rb;
    RaycastHit slopeHit;
    private float slopeAngle;

    private Vector2 _moveDirection;
    private bool isOnGround;
    public bool isGrounded
    {
        get => isOnGround;
    }
    private bool shouldJump = false;
    

    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.instance;
        if(gameManager != null)
            gameManager.GameStateChangedEvent += StopPlayerMovement;

        input.AddMoveEventListener(HandleMove);
        input.AddJumpEventListener(HandleJump);
        input.AddJumpCancelledEventListener(HandleCancelledJump);

        
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.GameStateChangedEvent -= StopPlayerMovement;

        input.RemoveMoveEventListener(HandleMove);
        input.RemoveJumpEventListener(HandleJump);
        input.RemoveJumpCancelledEventListener(HandleCancelledJump);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found!");
        }
        else
        {
            rb.maxAngularVelocity = currentMaxSpeed;
        }

    }

    private void FixedUpdate()
    {

        Move();
        

        if(hasJump)

            Jump();

        // Ground check
        if (Physics.SphereCast(transform.position, 0.4f, Vector3.down, out slopeHit, 0.2f))
            //if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.5f))
        {

            if (!isOnGround) previousGravity = temporaryGravity;

            isOnGround = true;
            temporaryGravity = standardGravity; //Reset gravity
            OnSlope();
        }
        else
        {
            isOnGround = false;
            previousGravity = temporaryGravity;
            IncreaseGravity();
            targetMaxSpeed = 28f; //Jumping doesn't preserve your speed
        }

        LerpSpeed();
        
    }

    private void Update()
    {

    }

    private void HandleMove(Vector2 dir)
    {
        _moveDirection = dir;
    }

    private void HandleJump()
    {
        shouldJump = true;
    }

    private void HandleCancelledJump()
    {
        shouldJump = false;
    }

    private void Move()
    {
        if (_moveDirection == Vector2.zero)
            return;

        rb.AddTorque(new Vector3(0, 0, -_moveDirection.y * moveAcceleration));
    }

    private void Jump()
    {
        
        if (shouldJump && isOnGround)
        {
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            shouldJump = false;
  
        }
    }

   

    private void OnSlope()
    {
       
        // Are you on a slope?
        if (slopeHit.normal.x > 0.05 || slopeHit.normal.x < -0.05)
        {

            // Are you going downhill? (This if might cause issues if you're going in reverse.)
            if (slopeHit.normal.x > 0.05 && currentMaxSpeed < maxDownSlopeSpeed && - _moveDirection.y == -1)
            {
                slopeAngle = slopeHit.normal.x;
                targetMaxSpeed = targetMaxSpeed + slopeAngle * downSlopeSpeedMultiplier;
                // Test to see if this works correctly
                Debug.DrawLine(transform.position, transform.position + slopeHit.normal * 5f, Color.green, 2f);
            }

            // Are you going uphill? (This if might cause issues if you're going in reverse.)
            if (slopeHit.normal.x < -0.05 && currentMaxSpeed > minUpSlopeSpeed)
            {
                slopeAngle = slopeHit.normal.x;
                targetMaxSpeed = targetMaxSpeed + slopeAngle * upSlopeSpeedMultiplier;
                // Test to see if this works correctly
                Debug.DrawLine(transform.position, transform.position + slopeHit.normal * 5f, Color.red, 2f);
            }

        }
        else // You're on flat ground
        {
            targetMaxSpeed = 28f;
            // Test to see if this works correctly
            Debug.DrawLine(transform.position, transform.position + slopeHit.normal * 5f, Color.blue, 2f);
        }
               
        
    }

    // This needs to be reworked... but how?
    private void LerpSpeed()
    {

        currentMaxSpeed = Mathf.Lerp(currentMaxSpeed, targetMaxSpeed, fastSpeedRampUpFactor * Time.fixedDeltaTime);

        if (rb.angularVelocity.magnitude + 3f > targetMaxSpeed && isOnGround)
        {
            currentOverSpeed = Mathf.Lerp(currentOverSpeed, maxOverSpeed, slowSpeedRampUpFactor * Time.fixedDeltaTime);
        }
        else
        {
            currentOverSpeed = Mathf.Lerp(currentOverSpeed, 0f, fastSpeedRampUpFactor * Time.fixedDeltaTime);
        }

        rb.maxAngularVelocity = currentMaxSpeed + currentOverSpeed;

    }

    private void IncreaseGravity()
    {
        temporaryGravity += gravityIncreaseFactor * Time.fixedDeltaTime;
        temporaryGravity = temporaryGravity > maximumGravity ? maximumGravity : temporaryGravity;
        Physics.gravity = new Vector3(0f, -temporaryGravity, 0f);
    }

    private async void StopPlayerMovement(GameManager.GameState state)
    {
        if (state != GameManager.GameState.Menu)
        {
            return;
        }

        float timeElapsed = 0f;
        Vector3 initialVelocity = rb.velocity;
        while (timeElapsed < .2f)
        {
            float t = timeElapsed / .2f;
            rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, t);
            await Task.Yield(); // This allows the loop to yield control to Unity's main thread, preventing it from blocking the game.
            timeElapsed += Time.deltaTime;
        }
        rb.isKinematic = true; // Set the rigidbody to kinematic to ensure it stops completely.
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
           // isOnGround = true;



        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
           // isOnGround = false;

        }
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    private SerializedProperty hasJumpProp;
    private SerializedProperty jumpHeightProp;

    private void OnEnable()
    {
        hasJumpProp = serializedObject.FindProperty("hasJump");
        jumpHeightProp = serializedObject.FindProperty("jumpHeight");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (hasJumpProp.boolValue)
        {
            EditorGUILayout.PropertyField(jumpHeightProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
