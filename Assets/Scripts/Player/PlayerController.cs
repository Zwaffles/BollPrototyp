using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;

    [SerializeField, Header("Movement")]
    private float moveAcceleration = 100f;
    // [SerializeField] Not editable
    private float currentMaxSpeed = 28f; // Renamed from moveSpeed
    [SerializeField]
    private float downSlopeSpeedMultiplier = 4;
    [SerializeField]
    private float maxDownSlopeSpeed = 80;
    [SerializeField]
    private float upSlopeSpeedMultiplier = 0.5f;
    [SerializeField]
    private float minUpSlopeSpeed = 10;

    // Variables for Ludwig's ramping speed suggestion - Johan
    [SerializeField, Header("Speed Ramping"), Tooltip("How quickly the speed increases when you're BELOW the regular max speed.")]
    private float fastSpeedRampUpFactor = 2f;
    [SerializeField, Tooltip("How quickly the speed increases when you're ABOVE the regular max speed.")]
    private float slowSpeedRampUpFactor = 0.15f;
    [SerializeField, Tooltip("How much above the regular max speed you can go.")]
    private float maxOverSpeed = 50f;
    private float currentOverSpeed = 0f;
    // [SerializeField, Tooltip("Max-speed the ball should lerp towards")] Not editable
    private float targetMaxSpeed = 28f;
    [SerializeField, Tooltip("Max speed to lerp towards for the ball on flat ground and in the air")]
    private float regularMaxSpeed = 28f;

    // Gravity variables
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

    // Moved these down to make the UI-cleaner

    // Jump variables
    [SerializeField, Header("Jump")]
    private bool hasJump = true;
    [SerializeField, HideInInspector]
    private float jumpHeight = 2f;

    // Boost variables - PS. Andreas, jag har inte lagt till de här variablerna till Region Public Fields
    [SerializeField]
    private bool hasBoost = false;
    [SerializeField, HideInInspector]
    private int numberOfBoosts = 3;
    [SerializeField, HideInInspector]
    private float boostSpeedFactor = 2f;
    [SerializeField, HideInInspector]
    private float boostRocketFactor = 2f;
    [SerializeField, HideInInspector]
    private float boostDuration = 2f;
    [SerializeField, HideInInspector]
    private float boostGravityFactor = 0f;
    private float remainingBoostDuration;
    private bool isBoosting = false;
    [SerializeField]
    private GameObject boostVisualiserObject;

    #region Public Fields
    public float MoveAcceleration { get => moveAcceleration; set => moveAcceleration = value; }
    public float CurrentMaxSpeed { get => currentMaxSpeed; set => currentMaxSpeed = value; }
    public float DownSlopeSpeedMultiplier { get => downSlopeSpeedMultiplier; set => downSlopeSpeedMultiplier = value; }
    public float MaxDownSlopeSpeed { get => maxDownSlopeSpeed; set => maxDownSlopeSpeed = value; }
    public float UpSlopeSpeedMultiplier { get => upSlopeSpeedMultiplier; set => upSlopeSpeedMultiplier = value; }
    public float MinUpSlopeSpeed { get => minUpSlopeSpeed; set => minUpSlopeSpeed = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float FastSpeedRampUpFactor { get => fastSpeedRampUpFactor; set => fastSpeedRampUpFactor = value; }
    public float SlowSpeedRampUpFactor { get => slowSpeedRampUpFactor; set => slowSpeedRampUpFactor = value; }
    public float MaxOverSpeed { get => maxOverSpeed; set => maxOverSpeed = value; }
    public float TargetMaxSpeed { get => targetMaxSpeed; set => targetMaxSpeed = value; }
    public float StandardGravity { get => standardGravity; set => standardGravity = value; }
    public float GravityIncreaseFactor { get => gravityIncreaseFactor; set => gravityIncreaseFactor = value; }
    public float MaximumGravity { get => maximumGravity; set => maximumGravity = value; }
    public float BoostSpeedFactor { get => boostSpeedFactor; set => boostSpeedFactor = value; }
    public float BoostDuration { get => boostDuration; set => boostDuration = value; }
    public float BoostGravityFactor { get => boostGravityFactor; set => boostGravityFactor = value; }
    public float BoostRocketFactor { get => boostRocketFactor; set => boostRocketFactor = value; }
    #endregion

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

        remainingBoostDuration = boostDuration;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + Vector3.down, 0.4f);
    }
    private void FixedUpdate()
    {

        Move();
        

        if(hasJump)
            
            Jump();

        if (hasBoost)

            Boost();


        // Ground check
        if (Physics.SphereCast(transform.position, 0.4f, Vector3.down, out slopeHit, 0.2f))
           // if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.5f))
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
            targetMaxSpeed = regularMaxSpeed; //Jumping doesn't preserve your speed
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

    private void Boost()
    {
        
        if (isBoosting)
        {

            rb.AddForce(rb.velocity.normalized * boostRocketFactor);

            remainingBoostDuration -= Time.fixedDeltaTime;
            if (remainingBoostDuration < 0f)
            {
                isBoosting = false;
                remainingBoostDuration = boostDuration;
                boostVisualiserObject.SetActive(false);
            }
            
            return;

        }

        if (shouldJump && numberOfBoosts > 0) // Used to have && !isBoosting too, but that became redundant
        {

            shouldJump = false;
            numberOfBoosts--;
            isBoosting = true;
            boostVisualiserObject.SetActive(true);

        }

    }

    private void OnSlope()
    {
       
        // Are you on a slope?
        if (slopeHit.normal.x > 0.05 || slopeHit.normal.x < -0.05)
        {

            // Are you going downhill? (This if might cause issues if you're going in reverse.)
            if (slopeHit.normal.x > 0.05 && targetMaxSpeed < maxDownSlopeSpeed)
            {
                if(-_moveDirection.y == -1)
                {
                    slopeAngle = slopeHit.normal.x;
                targetMaxSpeed = targetMaxSpeed + slopeAngle * downSlopeSpeedMultiplier;
                // Test to see if this works correctly
                Debug.DrawLine(transform.position, transform.position + slopeHit.normal * 5f, Color.green, 2f);
            }
                if (-_moveDirection.y == 1)
                {
                    slopeAngle = slopeHit.normal.x;
                    targetMaxSpeed = targetMaxSpeed + slopeAngle * upSlopeSpeedMultiplier;
                    // Test to see if this works correctly
                    Debug.DrawLine(transform.position, transform.position + slopeHit.normal * 5f, Color.red, 2f);
                }
            }

            // Are you going uphill? (This if might cause issues if you're going in reverse.)

            if (slopeHit.normal.x < -0.05 && targetMaxSpeed > minUpSlopeSpeed)
            {
                if (-_moveDirection.y == 1)
                {
                    slopeAngle = slopeHit.normal.x;
                    targetMaxSpeed = targetMaxSpeed + slopeAngle * downSlopeSpeedMultiplier;
                    // Test to see if this works correctly
                    Debug.DrawLine(transform.position, transform.position + slopeHit.normal * 5f, Color.green, 2f);
                }

                if (-_moveDirection.y == -1)
                {
                    slopeAngle = slopeHit.normal.x;
                    targetMaxSpeed = targetMaxSpeed + slopeAngle * upSlopeSpeedMultiplier;
                    // Test to see if this works correctly
                    Debug.DrawLine(transform.position, transform.position + slopeHit.normal * 5f, Color.red, 2f);
                }
            }

        }
        else // You're on flat ground
        {
            targetMaxSpeed = regularMaxSpeed;
            // Test to see if this works correctly
            Debug.DrawLine(transform.position, transform.position + slopeHit.normal * 5f, Color.blue, 2f);
        }
               
        
    }

    // This needs to be reworked... but how?
    private void LerpSpeed()
    {

        currentMaxSpeed = Mathf.Lerp(
            currentMaxSpeed, 
            isBoosting ? targetMaxSpeed * boostSpeedFactor : targetMaxSpeed, 
            fastSpeedRampUpFactor * Time.fixedDeltaTime
            );

        if (rb.angularVelocity.magnitude + 3f > targetMaxSpeed && isOnGround)
        {
            currentOverSpeed = Mathf.Lerp(currentOverSpeed, maxOverSpeed, slowSpeedRampUpFactor * Time.fixedDeltaTime);
        }
        else
        {
            currentOverSpeed = Mathf.Lerp(currentOverSpeed, 0f, fastSpeedRampUpFactor * Time.fixedDeltaTime);
        }

        if (-_moveDirection.y == 1)
        {
            rb.maxAngularVelocity = currentMaxSpeed / 1.2f + currentOverSpeed;
        }
        else
        {
            rb.maxAngularVelocity = currentMaxSpeed + currentOverSpeed;
        }

    }

    private void IncreaseGravity()
    {
        temporaryGravity += gravityIncreaseFactor * Time.fixedDeltaTime;
        temporaryGravity = temporaryGravity > maximumGravity ? maximumGravity : temporaryGravity;
        Physics.gravity = new Vector3(
            0f, 
            isBoosting ? -temporaryGravity * boostGravityFactor : -temporaryGravity, 
            0f
            );
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

    private SerializedProperty hasBoostProp;
    private SerializedProperty numberOfBoostsProp;
    private SerializedProperty boostSpeedFactorProp;
    private SerializedProperty boostDurationProp;
    private SerializedProperty boostGravityFactorProp;
    private SerializedProperty boostRocketFactorProp;

    private void OnEnable()
    {

        hasJumpProp = serializedObject.FindProperty("hasJump");
        jumpHeightProp = serializedObject.FindProperty("jumpHeight");

        hasBoostProp = serializedObject.FindProperty("hasBoost");
        numberOfBoostsProp = serializedObject.FindProperty("numberOfBoosts");
        boostSpeedFactorProp = serializedObject.FindProperty("boostSpeedFactor");
        boostDurationProp = serializedObject.FindProperty("boostDuration");
        boostGravityFactorProp = serializedObject.FindProperty("boostGravityFactor");
        boostRocketFactorProp = serializedObject.FindProperty("boostRocketFactor");

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (hasJumpProp.boolValue)
        {
            EditorGUILayout.PropertyField(jumpHeightProp);
        }

        if (hasBoostProp.boolValue)
        {
            EditorGUILayout.PropertyField(numberOfBoostsProp);
            EditorGUILayout.PropertyField(boostSpeedFactorProp);
            EditorGUILayout.PropertyField(boostDurationProp);
            EditorGUILayout.PropertyField(boostGravityFactorProp);
            EditorGUILayout.PropertyField(boostRocketFactorProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
