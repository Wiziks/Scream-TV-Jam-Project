using UnityEngine;

public class PlayerMoving : MonoBehaviour {
    [Header("Moving")]
    [SerializeField][Range(1f, 5f)] private float _playerSpeed;

    [Header("Sprint")]
    [SerializeField][Range(1f, 5f)] private float _sprintSpeedMultiplier;
    [SerializeField][Range(0f, 10f)] private float _sprintMaxDuration;
    [SerializeField][Range(1f, 15f)] private float _sprintCooldown;

    [Header("Player Position")]
    [SerializeField] private PlayerPosition _playerPosition;

    [Header("Game Event")]
    [SerializeField] private GameEvent _onRestoring;

    [Header("Sounds")]
    [SerializeField] private AudioSource _stepAudioSource;
    [SerializeField] private float _stepRate;
    [SerializeField] private Vector2 _pitchRange;

    private float _stepTimer;

    private SprintState _sprintState;

    public bool CanMoving { get => _canMoving; set => _canMoving = value; }
    private bool _canMoving;

    private float _sprintingTime;
    private float _restoringTime;

    private bool _isMoving;
    private Vector3 _lastPosition;

    private void Awake() {
        _playerPosition.SetPlayerPosition(transform);
        _canMoving = true;
        _sprintState = SprintState.Ready;

        _lastPosition = transform.position;
    }

    public void OnMove(Component component, object data) {
        if (_canMoving == false) return;

        if (data is not Vector3 direction) return;

        Vector3 offset = transform.right * direction.x + transform.forward * direction.z;

        float speed = _playerSpeed;
        HandleSprintInput();

        if (_sprintState == SprintState.Sprinting)
            speed *= _sprintSpeedMultiplier;

        transform.position += offset * speed * Time.deltaTime;

        _isMoving = _lastPosition != transform.position;

        _lastPosition = transform.position;

        HandleStepSound(speed);
    }

    private void HandleSprintInput() {
        switch (_sprintState) {
            case SprintState.Ready:
                if (Input.GetKey(KeyCode.LeftShift)) {
                    _sprintState = SprintState.Sprinting;
                }
                break;
            case SprintState.Sprinting:
                _sprintingTime += Time.deltaTime;
                _restoringTime = Mathf.Lerp(_sprintCooldown, 0f, Mathf.Clamp01(_sprintingTime / _sprintMaxDuration));

                if (Input.GetKey(KeyCode.LeftShift) == false || _sprintingTime > _sprintMaxDuration) {
                    _sprintState = SprintState.Restoring;

                    _sprintingTime = 0f;
                }
                break;
            case SprintState.Restoring:
                _restoringTime += Time.deltaTime;

                if (_restoringTime > _sprintCooldown) {
                    _sprintState = SprintState.Ready;
                }
                break;
            default:
                break;
        }

        _onRestoring.Raise(_restoringTime / _sprintCooldown);
    }

    private void HandleStepSound(float speed) {
        if (_isMoving == false) return;

        _stepTimer -= Time.deltaTime * speed;

        if (_stepTimer <= 0f) {
            _stepAudioSource.pitch = Random.Range(_pitchRange.x, _pitchRange.y);
            _stepAudioSource.Play();
            _stepTimer = _stepRate;
        }
    }

    private enum SprintState {
        Ready,
        Sprinting,
        Restoring,
    }
}