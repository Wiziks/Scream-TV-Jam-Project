using UnityEngine;

public class PlayerMoveZone : MonoBehaviour {
    [SerializeField] private Vector2 _xRange;
    [SerializeField] private Vector2 _zRange;

    private void Start() {

    }

    private void LateUpdate() {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, _xRange.x, _xRange.y);
        position.z = Mathf.Clamp(position.z, _zRange.x, _zRange.y);

        transform.position = position;
    }
}
