using System.Collections;

using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private ZombieFactory _zombieFactory;
    [SerializeField] private Graph _graph;

    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;

    private Node[] _spawnNodes;

    private void Start() {
        _zombieFactory.GenerateMonsterPool();
        _spawnNodes = _graph.GetStartNodes();
        StartSpawn();
    }

    public void StartSpawn() {
        StartCoroutine(SpawnMonstersTimer());
    }

    private IEnumerator SpawnMonstersTimer() {
        while (true) {
            SpawnMonster();
            yield return new WaitForSeconds(Random.Range(_minSpawnTime, _maxSpawnTime));
        }
    }

    private void SpawnMonster() {
        _spawnNodes.Shuffle();
        for (int i = 0; i < _spawnNodes.Length; i++) {
            if (_spawnNodes[i].TryGetRandomEdge(out Edge edge)) {
                _zombieFactory.CreateMonster(edge);
                return;
            }
        }
    }
}
