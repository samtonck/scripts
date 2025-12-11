using System;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public event Action<GameObject> OnObjectHit;

    [SerializeField] private Camera _mainCamera;

    private void OnEnable()
    {
        var inputReader = GetComponent<InputReader>();
        inputReader.OnLeftMouseClick += PerformRaycast;
    }

    private void OnDisable()
    {
        var inputReader = GetComponent<InputReader>();
        inputReader.OnLeftMouseClick -= PerformRaycast;
    }

    private void PerformRaycast()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject hitObject = hit.collider.gameObject;
            OnObjectHit?.Invoke(hitObject);
        }
    }
}
