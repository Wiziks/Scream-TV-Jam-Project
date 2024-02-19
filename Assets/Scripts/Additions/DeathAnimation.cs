using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class DeathAnimation : MonoBehaviour {
    [SerializeField] private PlayerMoving _playerMoving;
    [SerializeField] private CameraMoving _cameraMoving;
    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _animationCurve;

    [SerializeField] private UnityEvent _onDeathEvent;

    public void OnChangeState(Component component, object data) {
        if (data is not Zombie.State state) return;
        if (state != Zombie.State.Killing) return;


        _playerMoving.CanMoving = false;
        _cameraMoving.CanMoving = false;

        StartCoroutine(CameraLookCoroutine());
    }

    private IEnumerator CameraLookCoroutine() {
        Vector3 targetDirection = Vector3.up;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        for (float t = 0f; t < 1f; t += Time.deltaTime / _duration) {
            float value = _animationCurve.Evaluate(t);
            print(t + " " + value);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, value);
            yield return null;
        }

        _onDeathEvent?.Invoke();
    }
}
