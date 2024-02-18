using System.Collections;

using UnityEngine;

public class Zombie : MonoBehaviour, IMonster {
    [Header("Walk")]
    [SerializeField] private Vector2 _walkTime;

    [Header("Hide")]
    [SerializeField] private Vector2 _hideTime;

    [Header("Attack")]
    [SerializeField] private float _attackSpeed;
    [SerializeField] private PlayerPosition _playerPosition;

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [Header("Collider")]
    [SerializeField] private float _colliderRadius;

    [Header("Game Event")]
    [SerializeField] private GameEvent _stateChanging;

    private State _state;
    public void Spawn(Edge startEdge) {
        gameObject.SetActive(true);

        StartCoroutine(Walking(startEdge));
    }

    private IEnumerator Walking(Edge edge) {
        SetState(State.Walk);

        _animator.SetBool("Walk", true);

        edge.BeginNode.IsEmpty = true;
        edge.DestNode.IsEmpty = false;

        float walkTime = Random.Range(_walkTime.x, _walkTime.y);

        _animator.speed = 10f / walkTime;

        for (float t = 0f; t < 1f; t += Time.deltaTime / walkTime) {
            transform.position = edge.GetPosition(t);
            Vector3 lookAtPoint = edge.GetPosition(t + 0.01f);
            lookAtPoint.y = transform.position.y;
            transform.LookAt(edge.GetPosition(t + 0.01f));
            yield return null;
        }

        transform.position = edge.GetPosition(1f);

        if (edge.DestNode.IsLast) {
            StartCoroutine(Attack());
            edge.DestNode.IsEmpty = true;
        } else {
            StartCoroutine(Hiding(edge.DestNode));
        }
    }

    private IEnumerator Hiding(Node node) {
        SetState(State.Hide);

        _animator.SetBool("Walk", false);

        transform.LookAt(transform.position - Vector3.forward);

        while (_state == State.Hide) {
            float hideTime = Random.Range(_hideTime.x, _hideTime.y);

            yield return new WaitForSeconds(hideTime);

            if (node.TryGetRandomEdge(out Edge edge)) {
                StartCoroutine(Walking(edge));
            }
        }
    }

    public IEnumerator Attack() {
        SetState(State.Attack);

        Vector3 target = _playerPosition.Position;

        _animator.SetTrigger("Run");

        _animator.speed = _attackSpeed / 2f;

        transform.LookAt(target);

        while (transform.position != target || _state != State.Attack) {
            transform.position = Vector3.MoveTowards(
                transform.position, target, _attackSpeed * Time.deltaTime);

            if (Vector3.Distance(_playerPosition.Position, transform.position) < _colliderRadius) {
                SetState(State.Killing);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void SetState(State newState) {
        _state = newState;
        _stateChanging.Raise(newState);
    }

    public enum State {
        Walk,
        Hide,
        Attack,
        Killing,
    }
}