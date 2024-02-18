using UnityEngine;

public class InputSystem : MonoBehaviour {
    [Header("Game Events")]
    [SerializeField] private GameEvent _onMove;
    [SerializeField] private GameEvent _onMouseMove;

    private void Update() {
        _onMove.Raise(GetMovingDirection());
        _onMouseMove.Raise(GetMouseDelta());
    }

    private Vector3 GetMovingDirection() {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) {
            direction.z += 1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            direction.z -= 1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            direction.x += 1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            direction.x -= 1f;
        }

        return direction.normalized;
    }

    private Vector2 GetMouseDelta() =>
        new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
}
