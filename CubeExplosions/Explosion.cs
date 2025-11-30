using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 100f;
    [SerializeField] private float _explosionForce = 2500f;
    [SerializeField] private float _scaleMultiplier = 2f;

    public IEnumerator ExplodeAndScatterClones(float delay, List<GameObject> clones)
    {
        Vector3 explosionPosition = transform.position;
        
        yield return new WaitForSeconds(delay);
        
        Debug.Log($"Взрыв с делением! Раскидываем {clones.Count} клонов");
        
        foreach (GameObject clone in clones)
        {
            if (clone != null)
            {
                Rigidbody cloneRigidbody = clone.GetComponent<Rigidbody>();
                cloneRigidbody.AddExplosionForce(_explosionForce, explosionPosition, _explosionRadius);
            }
        }
    }

    public IEnumerator ExplodeArea(float delay)
    {
        Vector3 explosionPosition = transform.position;
        float scale = transform.localScale.x;
        
        float scaledRadius = _explosionRadius / scale * _scaleMultiplier;
        float scaledForce = _explosionForce / scale * _scaleMultiplier;
        
        yield return new WaitForSeconds(delay);
        
        List<Rigidbody> explodableObjects = GetExplodableObjects(explosionPosition, scaledRadius);
        
        Debug.Log($"Взрыв без деления! Scale: {scale:F2}, Радиус: {scaledRadius:F0}, Сила: {scaledForce:F0}, Объектов: {explodableObjects.Count}");
        
        foreach (Rigidbody explodableObject in explodableObjects)
        {
            explodableObject.AddExplosionForce(scaledForce, explosionPosition, scaledRadius);
        }
    }

    private List<Rigidbody> GetExplodableObjects(Vector3 explosionPosition, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(explosionPosition, radius);

        List<Rigidbody> items = new List<Rigidbody>();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null && hit.attachedRigidbody.gameObject != gameObject)
            {
                items.Add(hit.attachedRigidbody);
            }
        }

        return items;
    }
}

