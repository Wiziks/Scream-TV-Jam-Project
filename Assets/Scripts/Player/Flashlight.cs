using System.Collections;

using UnityEngine;

public class Flashlight : MonoBehaviour {
    [Header("Light")]
    [SerializeField] private float _lightRadius;
    [SerializeField] private float _lightCooldown;
    [SerializeField] private Light _light;
    [SerializeField] private Material _material;

    [Header("Animation")]
    [SerializeField] private AnimationCurve _turnOffAnimation;
    [SerializeField] private float _turnOffDuration;

    [Header("Zombie Factory")]
    [SerializeField] private ZombieFactory _zombieFactory;

    [Header("Game Event")]
    [SerializeField] private GameEvent _flashlightTimer;

    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;

    private float _timer;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && _timer >= _lightCooldown) {
            TurnOn();
        }

        _timer += Time.deltaTime;

        _flashlightTimer.Raise(Mathf.Clamp01(_timer / _lightCooldown));
    }

    private void TurnOn() {
        Vector3 end = transform.parent.position;
        end.y = 0f;

        Vector3 start = transform.parent.position + transform.parent.forward * _lightRadius;
        start.y = 0f;

        Debug.DrawLine(start, end, Color.red, 5f);
        // print(start + " " + end);

        Zombie[] zombies = _zombieFactory.ZombieCollisionIntersection(start, end);
        foreach (Zombie zombie in zombies) {
            zombie.KillZombie();
        }

        _light.enabled = true;

        _timer = 0f;

        _audioSource.Play();

        StartCoroutine(TurnOff());
    }

    private IEnumerator TurnOff() {
        float maxIntensity = _light.intensity;

        for (float t = 0f; t < 1f; t += Time.deltaTime / _turnOffDuration) {
            _light.intensity = Mathf.Lerp(0f, maxIntensity, _turnOffAnimation.Evaluate(t));
            _material.EnableKeyword("_EMISSION");
            _material.SetColor("_EmissionColor", new Color(0.75f, 0.75f, 0.75f) * Mathf.Lerp(0f, 2, _turnOffAnimation.Evaluate(t)));
            yield return null;
        }

        _light.enabled = false;

        _light.intensity = maxIntensity;
    }
}
