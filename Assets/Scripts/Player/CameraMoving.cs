using UnityEngine;

public class CameraMoving : MonoBehaviour {
    [Header("Mouse Settings")]
    [SerializeField] private float _sensitivity;
    [SerializeField][Range(0f, 90f)] private float _maxLookXAngle;

    [Header("Look At Settings")]
    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] private float _lookAtSmoothFactor;

    private Transform _cameraTransform;
    private float xRotation = 0f;

    public bool CanMoving { get => _canMoving; set => _canMoving = value; }
    private bool _canMoving;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cameraTransform = Camera.main.transform;
        _canMoving = true;
    }

    public void OnMouseMove(Component component, object data) {
        if (_canMoving == false) return;

        if (data is not Vector2 delta) return;

        float mouseX = delta.x * _sensitivity * Time.deltaTime;
        float mouseY = delta.y * _sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -_maxLookXAngle, _maxLookXAngle);

        _cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}