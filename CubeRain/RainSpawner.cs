using System.Collections;
using UnityEngine;

public class RainSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private RainItem _itemPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int _initialPoolSize = 10;
    [SerializeField] private int _expandSize = 5;
    [SerializeField] private Transform _poolContainer;

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private Color _initialColor = Color.white;

    [Header("Spawn Area")]
    [SerializeField] private float _positionMinX = -5f;
    [SerializeField] private float _positionMaxX = 5f;
    [SerializeField] private float _spawnHeight = 15f;
    [SerializeField] private float _positionMinZ = -5f;
    [SerializeField] private float _positionMaxZ = 5f;

    [Header("Item Randomization")]
    [SerializeField] private float _minRotationAngle = 0f;
    [SerializeField] private float _maxRotationAngle = 360f;
    [SerializeField] private float _minScale = 0.1f;
    [SerializeField] private float _maxScale = 0.5f;

    private ObjectPool<RainItem> _pool;

    private void Start()
    {
        InitializePool();
        StartCoroutine(SpawnRoutine());
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<RainItem>(_itemPrefab, _initialPoolSize, _expandSize, _poolContainer);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnItem()
    {
        RainItem item = _pool.Get();
        item.transform.position = GetRandomSpawnPosition();
        item.transform.rotation = GetRandomRotation();
        item.transform.localScale = GetRandomScale();
        
        Rigidbody rigidbody = item.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        item.LifetimeExpired -= OnItemLifetimeExpired;
        item.LifetimeExpired += OnItemLifetimeExpired;
        
        item.Initialize(_initialColor);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(_positionMinX, _positionMaxX);
        float z = Random.Range(_positionMinZ, _positionMaxZ);
        return new Vector3(x, _spawnHeight, z);
    }

    private Quaternion GetRandomRotation()
    {
        float x = Random.Range(_minRotationAngle, _maxRotationAngle);
        float y = Random.Range(_minRotationAngle, _maxRotationAngle);
        float z = Random.Range(_minRotationAngle, _maxRotationAngle);
        return Quaternion.Euler(x, y, z);
    }

    private Vector3 GetRandomScale()
    {
        float scale = Random.Range(_minScale, _maxScale);
        return new Vector3(scale, scale, scale);
    }

    private void OnItemLifetimeExpired(RainItem item)
    {
        item.LifetimeExpired -= OnItemLifetimeExpired;
        _pool.Return(item);
    }
}

