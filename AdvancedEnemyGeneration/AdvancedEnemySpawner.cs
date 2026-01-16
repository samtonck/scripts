using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemySpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private AdvancedEnemy _enemyPrefab;
    [SerializeField] private Chiken _enemyTargetPrefab;

    [Header("List Path Points Chiken")]
    [SerializeField] private Transform[] _chikenPoints;

    [Header("Pool Settings")]
    [SerializeField] private int _initialPoolSize = 10;
    [SerializeField] private int _expandSize = 5;

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnInterval = 2f;

    private List<AdvancedEnemy> _activeEnemies = new List<AdvancedEnemy>();
    private ObjectPool<AdvancedEnemy> _pool;
    private Transform _poolContainer;
    private Chiken _chiken;

    private void Start()
    {
        SpawnChiken();
        CreatePoolContainer();
        InitializePool();
        StartCoroutine(SpawnRoutine());
    }

    private void CreatePoolContainer()
    {
        _poolContainer = new GameObject($"PoolContainer").transform;
        _poolContainer.SetParent(transform);
        _poolContainer.localPosition = Vector3.zero;
    }

    private void OnDestroy()
    {
        foreach (AdvancedEnemy enemy in _activeEnemies)
        {
            if (enemy != null)
            {
                enemy.AdvancedEnemyReachedPoint -= OnEnemyWayPointReached;
            }
        }

        _activeEnemies.Clear();
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<AdvancedEnemy>(_enemyPrefab, _initialPoolSize, _expandSize, _poolContainer);
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

    private void SpawnChiken()
    {
        _chiken = UnityEngine.Object.Instantiate(_enemyTargetPrefab);
        _chiken.transform.position = _chikenPoints[0].transform.position;
        _chiken.Initialize(_chikenPoints);
    }

    private void SpawnEnemy()
    {
        if (_chiken == null)
            return;

        AdvancedEnemy enemy = _pool.Get();
        enemy.transform.position = transform.position;

        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.Rigidbody.angularVelocity = Vector3.zero;

        enemy.AdvancedEnemyReachedPoint += OnEnemyWayPointReached;
        _activeEnemies.Add(enemy);

        enemy.Initialize(_chiken.transform);
    }

    private void OnEnemyWayPointReached(AdvancedEnemy enemy)
    {
        enemy.AdvancedEnemyReachedPoint -= OnEnemyWayPointReached;
        _activeEnemies.Remove(enemy);
        _pool.Return(enemy);
    }
}
