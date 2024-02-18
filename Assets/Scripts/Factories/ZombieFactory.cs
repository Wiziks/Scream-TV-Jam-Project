using UnityEngine;

public class ZombieFactory : MonoBehaviour, IMonsterFactory {
    [SerializeField] private Zombie _zombiePrefab;
    [SerializeField][Min(1)] private int _zombiePool;
    private Zombie[] _zombies;
    private int _zombiePoolIndex;

    public void CreateMonster(Edge startEdge) {
        _zombies[_zombiePoolIndex++ % _zombiePool].Spawn(startEdge);
    }

    public void GenerateMonsterPool() {
        _zombies = new Zombie[_zombiePool];

        for (int i = 0; i < _zombiePool; i++) {
            _zombies[i] = Instantiate(_zombiePrefab, transform);
            _zombies[i].gameObject.SetActive(false);
        }

        _zombiePoolIndex = 0;
    }
}
