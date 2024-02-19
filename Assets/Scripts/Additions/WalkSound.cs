using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WalkSound : MonoBehaviour {
    [SerializeField] private Vector2 _pitchRange;
    private AudioSource _walkAudioSource;

    public void MakeSound() {
        if (_walkAudioSource == null) {
            _walkAudioSource = GetComponent<AudioSource>();
        }

        _walkAudioSource.pitch = Random.Range(_pitchRange.x, _pitchRange.y);
        _walkAudioSource.Play();
    }
}
