using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Enemy _enemyPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int _initialPoolSize = 10;
    [SerializeField] private int _expandSize = 5;
    [SerializeField] private Transform _poolContainer;

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnInterval = 2f;

    [Header("Spawn Area")]
    [SerializeField] private float _positionMinX = -5f;
    [SerializeField] private float _positionMaxX = 5f;
    [SerializeField] private float _spawnHeight = 0f;
    [SerializeField] private float _positionMinZ = -5f;
    [SerializeField] private float _positionMaxZ = 5f;

    private ObjectPool<Enemy> _pool;
    private List<Enemy> _activeEnemies = new List<Enemy>();

    private void Start()
    {
        InitializePool();
        StartCoroutine(SpawnRoutine());
    }

    private void OnDestroy()
    {
        foreach (Enemy enemy in _activeEnemies)
        {
            if (enemy != null)
            {
                enemy.ReachedPoint -= OnEnemyWayPointReached;
            }
        }

        _activeEnemies.Clear();
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Enemy>(_enemyPrefab, _initialPoolSize, _expandSize, _poolContainer);
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            SpawnEnemy();
            yield return wait;
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = _pool.Get();
        enemy.transform.position = GetRandomSpawnPosition();

        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.Rigidbody.angularVelocity = Vector3.zero;

        enemy.ReachedPoint += OnEnemyWayPointReached;
        _activeEnemies.Add(enemy);

        Vector3 randomDirection = GetRandomDirection();
        enemy.Initialize(randomDirection);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(_positionMinX, _positionMaxX);
        float z = Random.Range(_positionMinZ, _positionMaxZ);
        return new Vector3(x, _spawnHeight, z);
    }

    private Vector3 GetRandomDirection()
    {
        float randomYRotation = Random.Range(0f, 360f);
        return Quaternion.Euler(0, randomYRotation, 0) * Vector3.forward;
    }

    private void OnEnemyWayPointReached(Enemy enemy)
    {
        enemy.ReachedPoint -= OnEnemyWayPointReached;
        _activeEnemies.Remove(enemy);
        _pool.Return(enemy);
    }
}
