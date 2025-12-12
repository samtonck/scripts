using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Handler))]
public class Explosion : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 10f;
    [SerializeField] private float _explosionForce = 1000f;

    private Handler _handler;

    public event Action<GameObject> ObjectExploded;

    private void Awake()
    {
        _handler = GetComponent<Handler>();
    }

    private void OnEnable()
    {
        _handler.Exploded += HandleExplode;
    }

    private void OnDisable()
    {
        _handler.Exploded -= HandleExplode;
    }

    private List<GameObject> FindObjectsInRadius(Vector3 explosionCenter)
    {
        List<GameObject> objects = new List<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent(out ExplosionData explosionData))
                objects.Add(hit.gameObject);
        }

        return objects;
    }

    private void ApplyForceToObject(Vector3 explosionCenter, GameObject targetObject)
    {
        if (!targetObject.TryGetComponent(out Rigidbody rigidbody))
            return;

        Vector3 objectPosition = rigidbody.transform.position;
        Vector3 direction = objectPosition - explosionCenter;
        float distance = direction.magnitude;

        if (distance < Mathf.Epsilon) 
            return;

        direction.Normalize();

        float percentage = 1f - Mathf.Clamp01(distance / _explosionRadius);
        float force = _explosionForce * percentage;

        rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }

    private void ApplyExplosionToObjects(Vector3 explosionCenter, List<GameObject> objects)
    {
        foreach (GameObject targetObject in objects)
        {
            ApplyForceToObject(explosionCenter, targetObject);
        }
    }

    private void HandleExplode(GameObject targetObject)
    {
        Vector3 explosionCenter = targetObject.transform.position;
        List<GameObject> objects = FindObjectsInRadius(explosionCenter);
        ApplyExplosionToObjects(explosionCenter, objects);
        ObjectExploded?.Invoke(targetObject);
    }
}