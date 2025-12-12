using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 10f;
    [SerializeField] private float _explosionForce = 1000f;

    public void ExplodeObjects(Vector3 explosionCenter, List<ExplosionObject> objects)
    {
        foreach (ExplosionObject explosionObject in objects)
        {
            ApplyForceToObject(explosionCenter, explosionObject);
        }
    }

    public void Explode(ExplosionObject explosionObject)
    {
        Vector3 explosionCenter = explosionObject.transform.position;
        List<ExplosionObject> objects = FindObjectsInRadius(explosionCenter);
        ExplodeObjects(explosionCenter, objects);
    }

    private List<ExplosionObject> FindObjectsInRadius(Vector3 explosionCenter)
    {
        List<ExplosionObject> objects = new List<ExplosionObject>();
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent(out ExplosionObject explosionObject))
                objects.Add(explosionObject);
        }

        return objects;
    }

    private void ApplyForceToObject(Vector3 explosionCenter, ExplosionObject explosionObject)
    {
        if (!explosionObject.TryGetComponent(out Rigidbody rigidbody))
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
}