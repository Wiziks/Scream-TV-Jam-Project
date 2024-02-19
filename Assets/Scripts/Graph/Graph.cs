using System.Collections.Generic;

using UnityEngine;

public class Graph : MonoBehaviour {
    [SerializeField] private Edge[] _edges;

    [Header("Trajectory View")]
    [SerializeField] private bool _show;
    [SerializeField] private LineRenderer _lineRendererPrefab;
    [SerializeField] private int _precision;

    private LineRenderer[] _lineRenderers;

    private void Awake() {
        _lineRenderers = new LineRenderer[_edges.Length];

        int i = 0;
        foreach (Edge edge in _edges) {
            edge.BeginNode.AddEdge(edge);

            if (_show) {
                _lineRenderers[i] = Instantiate(_lineRendererPrefab, transform);
                _lineRenderers[i].positionCount = _precision;
                i++;
            }
        }
    }

    private void Update() {
        if (_show) {
            ShowTrajectory();
        }
    }

    private void ShowTrajectory() {
        for (int i = 0; i < _edges.Length; i++) {
            Vector3[] points = new Vector3[_precision + 1];

            for (int j = 0; j < points.Length; j++) {
                points[j] = _edges[i].GetPosition(j / (float)_precision);
            }

            _lineRenderers[i].SetPositions(points);
        }
    }

    public Node[] GetStartNodes() {
        List<Node> _nodes = new List<Node>();

        foreach (Edge edge in _edges) {
            if (edge.BeginNode.IsStart)
                _nodes.Add(edge.BeginNode);
        }

        return _nodes.ToArray();
    }
}