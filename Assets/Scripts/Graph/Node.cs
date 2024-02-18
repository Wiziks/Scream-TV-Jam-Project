using UnityEngine;

public class Node : MonoBehaviour {
    public Vector3 Position => transform.position;

    [System.NonSerialized]
    private readonly System.Collections.Generic.List<Edge> _destEdges =
        new System.Collections.Generic.List<Edge>();

    public bool IsStartNode => _isStartNode;
    [SerializeField] private bool _isStartNode;

    public void AddEdge(Edge edge) {
        _destEdges.Add(edge);
    }

    public bool IsEmpty { get => _isEmpty; set => _isEmpty = value; }
    private bool _isEmpty = true;

    public bool IsLast => _destEdges.Count == 0;

    public bool TryGetRandomEdge(out Edge edge) {
        _destEdges.Shuffle();

        foreach (Edge item in _destEdges) {
            if (item.DestNode.IsEmpty) {
                edge = item;
                return true;
            }
        }

        edge = _destEdges[0];
        return false;
    }
}