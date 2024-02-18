using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoving : MonoBehaviour {
    [Header("Moving")]
    [SerializeField][Range(1f, 5f)] private float _playerSpeed;

    [Header("Sprint")]
    [SerializeField][Range(1f, 5f)] private float _sprintSpeedMultiplier;
    [SerializeField][Range(0f, 10f)] private float _sprintMaxDuration;
    [SerializeField][Range(1f, 15f)] private float _sprintCooldown;

    [Header("Player Position")]
    [SerializeField] private PlayerPosition _playerPosition;

    private CharacterController _characterController;

    private bool _isSprinting = false;
    private float _currentSprintDuration;
    private float _sprintCooldownTimer;

    public bool CanMoving { get => _canMoving; set => _canMoving = value; }
    private bool _canMoving;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        _playerPosition.SetPlayerPosition(transform);
        _canMoving = true;
    }

    public void OnMove(Component component, object data) {
        if (_canMoving == false) return;

        if (data is not Vector3 direction) return;

        Vector3 offset = transform.right * direction.x + transform.forward * direction.z;

        float speed = _playerSpeed;
        HandleSprintInput();

        if (_isSprinting)
            speed *= _sprintSpeedMultiplier;

        _characterController.Move(offset * speed * Time.deltaTime);
    }

    private void HandleSprintInput() {
        if (Input.GetKey(KeyCode.LeftShift) && _sprintCooldownTimer <= 0f) {
            StartSprinting();
        } else {
            StopSprinting();
        }

        if (_isSprinting) {
            _currentSprintDuration -= Time.deltaTime;
            if (_currentSprintDuration <= 0f) {
                _sprintCooldownTimer = _sprintCooldown;
                StopSprinting();
            }
        } else if (_sprintCooldownTimer > 0f) {
            _sprintCooldownTimer -= Time.deltaTime;
        }
    }

    private void StartSprinting() {
        if (_currentSprintDuration <= 0f && _sprintCooldownTimer <= 0f) {
            _currentSprintDuration = _sprintMaxDuration;
        }
        _isSprinting = true;
    }

    private void StopSprinting() {
        _isSprinting = false;

        // _sprintCooldownTimer = Mathf.Lerp(0f, _sprintCooldown, _currentSprintDuration / _sprintMaxDuration);

        _currentSprintDuration = 0f;
    }
}