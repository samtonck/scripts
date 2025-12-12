using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int _minCountClone = 2;
    [SerializeField] private int _maxCountClone = 6;

    public List<ExplosionObject> Spawn(ExplosionObject originalObject)
    {
        Vector3 originalPosition = originalObject.transform.position;
        Vector3 originalScale = originalObject.transform.localScale;
        float originalChance = originalObject.CurrentChance;
        
        int cloneCount = UnityEngine.Random.Range(_minCountClone, _maxCountClone + 1);
        List<ExplosionObject> spawnedObjects = new List<ExplosionObject>();

        for (int i = 0; i < cloneCount; i++)
        {
            ExplosionObject spawnedObject = Instantiate(originalObject);
            spawnedObject.Initialize(originalPosition, originalScale, originalChance);
            spawnedObjects.Add(spawnedObject);
        }

        return spawnedObjects;
    }
}