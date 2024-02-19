using System.Collections.Generic;

using UnityEngine;

public class ZombieFactory : MonoBehaviour, IMonsterFactory {
    [SerializeField] private Zombie _zombiePrefab;
    [SerializeField][Min(1)] private int _zombiePool;
    private Zombie[] _zombies;
    private int _zombiePoolIndex;

    public void CreateMonster(Edge startEdge) {
        if (_zombies == null || _zombies.Length == 0) {
            GenerateMonsterPool();
        }
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

    public Zombie[] ZombieCollisionIntersection(Vector3 center, float radius) {
        List<Zombie> zombies = new List<Zombie>();
        foreach (Zombie zombie in _zombies) {
            if (Vector3.Distance(zombie.transform.position, center) <= radius + zombie.ColliderRadius) {
                zombies.Add(zombie);
            }
        }
        return zombies.ToArray();
    }
}
