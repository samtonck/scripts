using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Cube _cubePrefab;

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

    private ObjectPool<Cube> _pool;
    private List<Cube> _activeCubes = new List<Cube>();

    private void Start()
    {
        InitializePool();
        StartCoroutine(SpawnRoutine());
    }

    private void OnDestroy()
    {
        foreach (Cube cube in _activeCubes)
        {
            if (cube != null)
            {
                cube.LifetimeExpired -= OnCubeLifetimeExpired;
            }
        }
        
        _activeCubes.Clear();
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Cube>(_cubePrefab, _initialPoolSize, _expandSize, _poolContainer);
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            SpawnCube();
            yield return wait;
        }
    }

    private void SpawnCube()
    {
        Cube cube = _pool.Get();
        cube.transform.position = GetRandomSpawnPosition();
        
        cube.Rigidbody.velocity = Vector3.zero;
        cube.Rigidbody.angularVelocity = Vector3.zero;

        cube.LifetimeExpired += OnCubeLifetimeExpired;
        _activeCubes.Add(cube);
        
        cube.Initialize(_initialColor);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(_positionMinX, _positionMaxX);
        float z = Random.Range(_positionMinZ, _positionMaxZ);
        return new Vector3(x, _spawnHeight, z);
    }

    private void OnCubeLifetimeExpired(Cube cube)
    {
        cube.LifetimeExpired -= OnCubeLifetimeExpired;
        _activeCubes.Remove(cube);
        _pool.Return(cube);
    }
}

