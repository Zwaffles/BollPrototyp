using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Header("Movement Values")]
    private float moveSpeed = 8.0f;
    [SerializeField]
    private float jumpForce = 8f;

    [Header("Ground Check")]
    [SerializeField]
    private float minAngle = -85f;
    [SerializeField]
    private float maxAngle = 85f;

    private PlayerInputs _playerInputs;

    private Rigidbody _rigidbody;

    private Vector2 _move = Vector2.zero;

    private bool _isGrounded;

    private void Awake()
    {
        _playerInputs = new PlayerInputs();

        _playerInputs.Gameplay.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
        _playerInputs.Gameplay.Move.canceled += ctx => _move = Vector2.zero;

        _playerInputs.Gameplay.Jump.performed += _ => Jump();
    }

    private void OnEnable()
    {
        _playerInputs.Enable();
    }

    private void OnDisable()
    {
        _playerInputs.Disable();
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _isGrounded = false;

        _rigidbody.AddForce(new Vector2(_move.x, 0) * moveSpeed);
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            float angle = Vector3.Angle(contact.normal, Vector3.up);
            if(angle < maxAngle && angle > minAngle)
            {
                _isGrounded = true;
                break;
            }
        }
    }
}
