using UnityEngine;

public class CameraMoving : MonoBehaviour {
    [Header("Mouse Settings")]
    [SerializeField] private float _sensitivity;
    [SerializeField][Range(0f, 90f)] private float _maxLookXAngle;
    [SerializeField][Range(0f, 90f)] private float _maxLookYAngle;

    [Header("Look At Settings")]
    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] private float _lookAtSmoothFactor;

    private Transform _cameraTransform;
    private float xRotation = 0f;
    private float yRotation = 0f;

    public bool CanMoving { get => _canMoving; set => _canMoving = value; }
    private bool _canMoving;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cameraTransform = Camera.main.transform;
        _canMoving = true;
    }

    // private void LateUpdate() {
    //     LookAtTarget();
    // }

    public void OnMouseMove(Component component, object data) {
        if (_canMoving == false) return;

        if (data is not Vector2 delta) return;

        float mouseX = delta.x * _sensitivity * Time.deltaTime;
        float mouseY = delta.y * _sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -_maxLookXAngle, _maxLookXAngle);

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -_maxLookYAngle - 180, _maxLookYAngle - 180);

        _cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    // private void LookAtTarget() {
    //     Vector3 directionToTarget = _lookAtTarget.position - _cameraTransform.position;
    //     Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

    //     _cameraTransform.rotation = Quaternion.Slerp(_cameraTransform.rotation, targetRotation, _lookAtSmoothFactor);

    //     Vector3 eulerAngles = _cameraTransform.eulerAngles;
    //     eulerAngles.x = Mathf.Clamp(eulerAngles.x, -_maxLookXAngle, _maxLookXAngle);
    //     _cameraTransform.eulerAngles = eulerAngles;
    // }
}