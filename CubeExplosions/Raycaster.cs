using System;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class Raycaster : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private InputReader _inputReader;

    public event Action<ExplosionObject> ObjectHit;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        _inputReader.InteractionPressed += PerformRaycast;
    }

    private void OnDisable()
    {
        _inputReader.InteractionPressed -= PerformRaycast;
    }

    private void PerformRaycast()
    {
        Camera camera = _mainCamera != null ? _mainCamera : Camera.main;
        if (camera == null) 
            return;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.TryGetComponent(out ExplosionObject explosionObject))
            {
                ObjectHit?.Invoke(explosionObject);
            }
        }
    }
}
