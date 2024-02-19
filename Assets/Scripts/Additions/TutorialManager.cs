using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour {
    [Header("Player")]
    [SerializeField] private PlayerMoving _playerMoving;
    [SerializeField] private CameraMoving _cameraMoving;
    [SerializeField] private GameObject _flashlight;
    [SerializeField] private GameEvent _onMove;

    [Header("Text")]
    [SerializeField] private TMPro.TextMeshProUGUI _text;
    [SerializeField] private string[] _textSequence;
    [SerializeField] private float _textDeltaTime;

    [Header("Text Change Animation")]
    [SerializeField] private AnimationCurve _changeAnimationCurve;
    [SerializeField] private float _changeTime;

    [Header("Zombie")]
    [SerializeField] private Zombie _zombie;
    [SerializeField] private Edge _startEdge;

    [Header("Camera")]
    [SerializeField] private float _zoomTime;
    [SerializeField] private float _targetFOV;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private AnimationCurve _zoomAnimationCurve;
    private Camera _camera;

    [Header("Spawner")]
    [SerializeField] private Spawner _spawner;

    private int _nextTextIndex;
    private bool _isWaitingForMove;

    private void Start() {
        _playerMoving.CanMoving = false;
        _nextTextIndex = 0;
        _camera = Camera.main;

        StartCoroutine(ChangeText(() => {
            Time.timeScale = 0.1f;
            _zombie.Spawn(_startEdge);
            _cameraMoving.CanMoving = false;
            StartCoroutine(CameraLookCoroutine(() => {
                Color color = _text.color;
                color.a = 1f;
                _text.color = color;
                _text.text = "WASD to move";
                _playerMoving.CanMoving = true;
                _isWaitingForMove = true;
            }));
        }));
    }

    public void OnMove(Component component, object data) {
        if (_isWaitingForMove == false) return;
        if (data is not Vector3 direction) return;
        if (direction == Vector3.zero) return;

        Time.timeScale = 1f;
        _spawner.StartSpawn();
        _camera.fieldOfView = 60f;
        _cameraMoving.CanMoving = true;
        _text.text = "";
        _flashlight.SetActive(true);

        Invoke(nameof(DestroyAll), 5f);

        _isWaitingForMove = false;
    }

    private void DestroyAll() {
        Destroy(_zombie);
        Destroy(gameObject);
    }

    private IEnumerator ChangeText(UnityAction onEndAction) {
        Color color;

        while (_nextTextIndex < _textSequence.Length) {
            _text.text = _textSequence[_nextTextIndex];

            for (float t = 0f; t < 1f; t += Time.deltaTime / _changeTime) {
                color = _text.color;
                color.a = _changeAnimationCurve.Evaluate(t);
                _text.color = color;
                yield return null;
            }

            color = _text.color;
            color.a = 1f;
            _text.color = color;

            yield return new WaitForSeconds(_textDeltaTime);

            for (float t = 1f; t > 0f; t -= Time.deltaTime / _changeTime) {
                color = _text.color;
                color.a = _changeAnimationCurve.Evaluate(t);
                _text.color = color;
                yield return null;
            }

            color = _text.color;
            color.a = 0f;
            _text.color = color;

            _nextTextIndex++;
        }

        onEndAction?.Invoke();
    }

    private IEnumerator CameraLookCoroutine(UnityAction onEndAction) {
        Vector3 targetDirection = _cameraTarget.position - _camera.transform.position;
        Quaternion startRotation = _camera.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / _changeTime) {
            float value = _zoomAnimationCurve.Evaluate(t);
            _camera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, value);
            _camera.fieldOfView = Mathf.Lerp(60f, _targetFOV, value);
            yield return null;
        }

        onEndAction?.Invoke();
    }
}
