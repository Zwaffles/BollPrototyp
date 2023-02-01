using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField, Tooltip("The speed at which the camera zooms")]
    private float zoomSpeed = 10f;
    [SerializeField, Tooltip("The minimum amount of zoom")]
    private float minZoom = 10f;
    [SerializeField, Tooltip("The maximum amount of zoom")]
    private float maxZoom = 60f;

    private float _currentZoom = 60f;
    private Camera _cam;

    private PlayerInputs _playerInputs;

    private void Awake()
    {
        _playerInputs = new PlayerInputs();
        _playerInputs.Gameplay.Zoom.performed += ctx => _currentZoom += ctx.ReadValue<Vector2>().y * zoomSpeed;
    }

    private void OnEnable()
    {
        _playerInputs.Enable();
        _cam = GetComponent<Camera>();
    }

    private void OnDisable()
    {
        _playerInputs.Disable();
    }

    private void Update()
    {
        _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);
        _cam.fieldOfView = _currentZoom;
    }
}
