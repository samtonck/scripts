using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T _prefab;
    private readonly Transform _container;
    private readonly Queue<T> _pool;
    private readonly int _expandSize;
    private int _totalCreated;

    public int TotalCreated => _totalCreated;
    public int AvailableCount => _pool.Count;

    public ObjectPool(T prefab, int initialSize = 0, int expandSize = 1, Transform container = null)
    {
        _prefab = prefab;
        _container = container;
        _pool = new Queue<T>();
        _expandSize = expandSize;
        _totalCreated = 0;

        if (initialSize > 0)
        {
            Expand(initialSize);
        }
    }

    public T Get()
    {
        if (_pool.Count == 0)
        {
            Expand(_expandSize);
        }

        T item = _pool.Dequeue();
        item.gameObject.SetActive(true);
        return item;
    }

    public void Return(T item)
    {
        item.gameObject.SetActive(false);
        _pool.Enqueue(item);
    }

    private void Expand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        T item = UnityEngine.Object.Instantiate(_prefab, _container);
        item.gameObject.SetActive(false);
        _pool.Enqueue(item);
        _totalCreated++;
    }
}

