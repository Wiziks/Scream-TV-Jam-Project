using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerPosition", menuName = "PlayerPosition", order = 0)]
public class PlayerPosition : ScriptableObject {
    public Vector3 Position => _playerPosition.position;

    private Transform _playerPosition;

    public void SetPlayerPosition(Transform transform) => _playerPosition = transform;
}