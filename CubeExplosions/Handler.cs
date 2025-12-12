using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Raycaster), typeof(Spawner), typeof(Explosion))]
public class Handler : MonoBehaviour
{
    private Raycaster _raycaster;
    private Spawner _spawner;
    private Explosion _explosion;

    private void Awake()
    {
        _raycaster = GetComponent<Raycaster>();
        _spawner = GetComponent<Spawner>();
        _explosion = GetComponent<Explosion>();
    }

    private void OnEnable()
    {
        _raycaster.ObjectHit += HandleClick;
    }

    private void OnDisable()
    {
        _raycaster.ObjectHit -= HandleClick;
    }

    private bool CanClone(float currentChance)
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        return randomValue < currentChance;
    }

    private void HandleClick(ExplosionObject explosionObject)
    {
        if (CanClone(explosionObject.CurrentChance))
        {
            Vector3 originalPosition = explosionObject.transform.position;
            List<ExplosionObject> spawnedObjects = _spawner.Spawn(explosionObject);
            _explosion.ExplodeObjects(originalPosition, spawnedObjects);
            Destroy(explosionObject.gameObject);
        }
        else
        {
            _explosion.Explode(explosionObject);
            Destroy(explosionObject.gameObject);
        }
    }
}
