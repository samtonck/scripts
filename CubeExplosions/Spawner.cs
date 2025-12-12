using System;
using UnityEngine;

[RequireComponent(typeof(Handler))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private int _minCountClone = 2;
    [SerializeField] private int _maxCountClone = 6;

    private Handler _handler;

    public event Action<GameObject> ObjectCloned;
    public event Action<GameObject> ObjectDestroyed;

    private void Awake()
    {
        _handler = GetComponent<Handler>();
    }

    private void OnEnable()
    {
        _handler.SpawnClones += HandleSpawn;
    }

    private void OnDisable()
    {
        _handler.SpawnClones -= HandleSpawn;
    }

    private void HandleSpawn(GameObject originalObject)
    {
        if (!originalObject.TryGetComponent(out ExplosionData originalData))
            return;

        Vector3 originalPosition = originalObject.transform.position;
        Vector3 originalScale = originalObject.transform.localScale;
        float originalChance = originalData.CurrentChance;
        
        ObjectDestroyed?.Invoke(originalObject);
        
        int cloneCount = UnityEngine.Random.Range(_minCountClone, _maxCountClone + 1);

        for (int i = 0; i < cloneCount; i++)
        {
            GameObject targetObject = Instantiate(originalObject);
            targetObject.transform.position = originalPosition;
            targetObject.transform.rotation = UnityEngine.Random.rotation;
            targetObject.transform.localScale = originalScale / 2f;
            
            if (targetObject.TryGetComponent(out ExplosionData explosionData))
            {
                explosionData.InitializeFromParent(originalChance);
            }
            
            if (targetObject.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.useGravity = true;
            }
            
            ObjectCloned?.Invoke(targetObject);
        }
    }
}