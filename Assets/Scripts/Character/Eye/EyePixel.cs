using UnityEngine;

public class EyePixel : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Material _material;
    private Color _color;
    private float _intensity = 1f;
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = GetComponent<MeshRenderer>().material;
        Apply();
    }

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = GetComponent<MeshRenderer>().material;
        _material.SetColor("_Color", _color);
        _material.SetColor("_EmissionColor", _color);
        _material.SetFloat("_Intensity", _intensity);
        _material.SetFloat("_EmissionIntensity", _intensity);
    }

    public void SetColor(Color color)
    {
        _color = color;
        if (_material != null) Apply();
    }
    public void SetIntensity(float intensity)
    {
        _material.SetFloat("_Intensity", intensity);
        _material.SetFloat("_EmissionIntensity", intensity);
        _intensity = intensity;
    }
    private void Apply()
    {
        _material.SetColor("_Color", _color);
        _material.SetColor("_EmissionColor", _color * _intensity);
    }
    public void Activate()
    {
        _meshRenderer.enabled = true;
    }

    public void Deactivate()
    {
        _meshRenderer.enabled = false;
    }
}
