using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private ZombieFactory _zombieFactory;
    [SerializeField] private Graph _graph;

    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private AnimationCurve _spawnTime;
    [SerializeField] private float _spawnTimeOffset;

    [SerializeField] private GameObject _scoreManager;

    private Node[] _spawnNodes;

    private float _spawnTimeValue;

    private bool _canSpawn;

    public void StartSpawn() {
        gameObject.SetActive(true);

        _spawnNodes = _graph.GetStartNodes();
        _spawnTimeValue = 0f;

        _scoreManager.SetActive(true);

        _canSpawn = true;

        SpawnMonstersTimer();
    }

    private void SpawnMonstersTimer() {
        if (_canSpawn == false) return;

        SpawnMonster();

        float cooldown = Mathf.Lerp(_minSpawnTime, _maxSpawnTime, _spawnTime.Evaluate(_spawnTimeValue));
        _spawnTimeValue += _spawnTimeOffset;

        Invoke(nameof(SpawnMonstersTimer), cooldown);
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

    public void StopSpawn() {
        _canSpawn = false;
        _zombieFactory.DisactiveZombie();
    }
}
