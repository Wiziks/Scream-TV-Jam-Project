using UnityEngine;

public class TVMaterialHandler : MonoBehaviour {
    [SerializeField] private Material _material;
    [Header("Change Settings")]
    [SerializeField] private Color _startColor;
    [SerializeField] private float _startSmoothness;
    [Header("Start Settings")]
    [SerializeField] private Color _changeColor;
    [SerializeField] private float _changeSmoothness;

    private void Start() {
        ResetMaterial();
    }

    public void ChangeMaterial() {
        _material.color = _changeColor;
        _material.SetFloat("_Glossiness", _changeSmoothness);
    }

    public void ResetMaterial() {
        _material.color = _startColor;
        _material.SetFloat("_Glossiness", _startSmoothness);
    }
}
