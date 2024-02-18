using UnityEngine;

[System.Serializable]
public class Edge {
    public Node BeginNode => _beginNode;

    [Header("Nodes")]
    [SerializeField] private Node _beginNode;

    public Node DestNode => _destNode;
    [SerializeField] private Node _destNode;

    [Header("Trajectory")]
    [SerializeField] private Transform _bPoint;
    [SerializeField] private Transform _cPoint;

    public Vector3 GetPosition(float u) =>
        GetBezierPoint(_beginNode.Position, _bPoint.position, _cPoint.position, _destNode.Position, Mathf.Clamp01(u));

    private Vector3 GetBezierPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float u) =>
        a * Mathf.Pow(1 - u, 3) + 3 * b * u * Mathf.Pow(1 - u, 2) +
            3 * c * Mathf.Pow(u, 2) * (1 - u) + d * Mathf.Pow(u, 3);
}