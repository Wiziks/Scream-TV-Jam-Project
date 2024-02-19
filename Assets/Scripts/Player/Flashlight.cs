using System.Collections;

using UnityEngine;

public class Flashlight : MonoBehaviour {
    [Header("Light")]
    [SerializeField] private float _lightRadius;
    [SerializeField] private float _lightCooldown;
    [SerializeField] private Light _light;

    [Header("Animation")]
    [SerializeField] private AnimationCurve _turnOffAnimation;
    [SerializeField] private float _turnOffDuration;

    [Header("Zombie Factory")]
    [SerializeField] private ZombieFactory _zombieFactory;

    [Header("Game Event")]
    [SerializeField] private GameEvent _flashlightTimer;

    private float _timer;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && _timer >= _lightCooldown) {
            TurnOn();
        }

        _timer += Time.deltaTime;

        _flashlightTimer.Raise(Mathf.Clamp01(_timer / _lightCooldown));
    }

    private void TurnOn() {
        Vector3 center = transform.position + transform.forward * _lightRadius;
        center.y = 0f;

        Zombie[] zombies = _zombieFactory.ZombieCollisionIntersection(center, _lightRadius);
        foreach (Zombie zombie in zombies) {
            zombie.KillZombie();
        }

        _light.enabled = true;

        _timer = 0f;

        StartCoroutine(TurnOff());
    }

    private IEnumerator TurnOff() {
        float maxIntensity = _light.intensity;

        for (float t = 0f; t < 1f; t += Time.deltaTime / _turnOffDuration) {
            _light.intensity = Mathf.Lerp(0f, maxIntensity, _turnOffAnimation.Evaluate(t));
            yield return null;
        }

        _light.enabled = false;

        _light.intensity = maxIntensity;
    }
}
