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

    public Zombie[] ZombieCollisionIntersection(Vector2 startPoint, Vector2 endPoint) {
        List<Zombie> zombies = new List<Zombie>();
        foreach (Zombie zombie in _zombies) {
            if (zombie.ZombieState == Zombie.State.Attack &&
                LineIntersectsCircle(startPoint, endPoint, zombie.transform.position, zombie.ColliderRadius)) {
                zombies.Add(zombie);
            }
        }
        return zombies.ToArray();
    }

    public void DisactiveZombie() {
        foreach (Zombie zombie in _zombies) {
            zombie.gameObject.SetActive(false);
        }
    }

    private bool LineIntersectsCircle(Vector2 startPoint, Vector2 endPoint, Vector2 center, float radius) {
        Vector2 lineDir = endPoint - startPoint;
        Vector2 toCenter = center - startPoint;
        float t = Vector2.Dot(toCenter, lineDir.normalized);

        Vector2 closestPoint;
        if (t <= 0.0f) {
            closestPoint = startPoint;
        } else if (t >= lineDir.magnitude) {
            closestPoint = endPoint;
        } else {
            closestPoint = startPoint + lineDir.normalized * t;
        }

        return (closestPoint - center).magnitude <= radius;
    }
}
