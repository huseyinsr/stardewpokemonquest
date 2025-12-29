using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarpetOnceHide : MonoBehaviour
{
    private Renderer _renderer;
    private Collider _collider;
    private bool _hidden = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        if (_hidden) return;

        _hidden = true;

        _renderer.enabled = false;  
        _collider.enabled = false; 
    }
}
