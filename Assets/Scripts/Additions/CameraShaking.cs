using System.Collections;

using UnityEngine;

public class CameraShaking : MonoBehaviour {
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeStrength;
    [SerializeField] private AnimationCurve _shakeCurve;

    [ContextMenu("Shake")]
    public void ShakeCamera() {
        StartCoroutine(CameraShakingCoroutine());
    }

    private IEnumerator CameraShakingCoroutine() {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < _shakeDuration) {
            elapsedTime += Time.deltaTime;
            float strength = _shakeCurve.Evaluate(elapsedTime / _shakeDuration);
            transform.localPosition = startPosition + Random.insideUnitSphere * strength * _shakeStrength;
            yield return null;
        }
        transform.localPosition = startPosition;
    }
}
