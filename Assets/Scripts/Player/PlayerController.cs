using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;

    [SerializeField, Header("Movement")]
    private float moveAcceleration = 100f;
    [SerializeField]
    private float moveSpeed = 28f;
    [SerializeField, Header("Jump")]
    private bool hasJump = false;
    [SerializeField, HideInInspector]
    private float jumpHeight = 2f;
   

    private Rigidbody rb;

    private Vector2 _moveDirection;
    private bool isOnGround;
    private bool shouldJump = false;

    private void OnEnable()
    {
        input.AddMoveEventListener(HandleMove);
        input.AddJumpEventListener(HandleJump);
        input.AddJumpCancelledEventListener(HandleCancelledJump);
    }

    private void OnDisable()
    {
        input.RemoveMoveEventListener(HandleMove);
        input.RemoveJumpEventListener(HandleJump);
        input.RemoveJumpCancelledEventListener(HandleCancelledJump);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = moveSpeed;
        
    }

    private void FixedUpdate()
    {
        Move();

        if(hasJump)
            Jump();
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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = false;
          

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
