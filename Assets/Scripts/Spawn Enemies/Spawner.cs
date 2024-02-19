using System.Collections;

using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private ZombieFactory _zombieFactory;
    [SerializeField] private Graph _graph;

    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;

    private Node[] _spawnNodes;

    public void StartSpawn() {
        gameObject.SetActive(true);

        _spawnNodes = _graph.GetStartNodes();

        SpawnMonstersTimer();
    }

    private void SpawnMonstersTimer() {
        SpawnMonster();

        float cooldown = Random.Range(_minSpawnTime, _maxSpawnTime);
        Invoke(nameof(SpawnMonstersTimer), cooldown);
    }

    private void SpawnMonster() {
        print("Spawn Monster");
        _spawnNodes.Shuffle();
        for (int i = 0; i < _spawnNodes.Length; i++) {
            if (_spawnNodes[i].TryGetRandomEdge(out Edge edge)) {
                _zombieFactory.CreateMonster(edge);
                print("Create Monster");
                i = _spawnNodes.Length;
                return;
            }

            print(i);
        }
    }
}
