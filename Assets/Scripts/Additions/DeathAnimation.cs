using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class DeathAnimation : MonoBehaviour {
    [SerializeField] private PlayerMoving _playerMoving;
    [SerializeField] private CameraMoving _cameraMoving;

    [SerializeField] private UnityEvent _onStartDeathAnimationEvent;
    [SerializeField] private UnityEvent _onEndDeathAnimationEvent;

    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private Transform _point;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _cameraParentTransform;

    [Header("Kill Sound")]
    [SerializeField] private AudioSource _killSound;

    public void OnChangeState(Component component, object data) {
        if (data is not Zombie.State state) return;
        if (state != Zombie.State.Killing) return;

        Kill();
    }

    [ContextMenu("Kill")]
    public void Kill() {
        _playerMoving.CanMoving = false;
        _cameraMoving.CanMoving = false;
        _killSound.Play();

        _cameraTransform.parent = null;

        _cameraParentTransform.position = _cameraTransform.position;
        _cameraParentTransform.rotation = _cameraTransform.rotation;

        _cameraTransform.parent = _cameraParentTransform;

        _onStartDeathAnimationEvent?.Invoke();
        StartCoroutine(CameraLookCoroutine());
    }

    private IEnumerator CameraLookCoroutine() {
        Quaternion startRotation = _cameraParentTransform.rotation;
        Vector3 startPosition = _cameraParentTransform.position;

        for (float t = 0f; t < 1f; t += Time.deltaTime / _duration) {
            float value = _animationCurve.Evaluate(t);
            _cameraParentTransform.position = Vector3.Lerp(startPosition, _point.position, value);
            _cameraParentTransform.rotation = Quaternion.Lerp(startRotation, _point.rotation, value);
            yield return null;
        }

        _cameraParentTransform.position = _point.position;
        _cameraParentTransform.rotation = _point.rotation;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _onEndDeathAnimationEvent?.Invoke();
    }
}
