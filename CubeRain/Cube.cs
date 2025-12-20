using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;
    [SerializeField] private float _minHeightBound = -10f;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _hasCollidedWithPlatform;
    private Coroutine _lifetimeCoroutine;

    public Rigidbody Rigidbody => _rigidbody;
    public event Action<Cube> LifetimeExpired;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }
        
        _hasCollidedWithPlatform = false;
        LifetimeExpired = null;
    }

    private void Update()
    {
        if (transform.position.y < _minHeightBound)
        {
            LifetimeExpired?.Invoke(this);
        }
    }

    public void Initialize(Color initialColor)
    {
        _renderer.material.color = initialColor;
        _hasCollidedWithPlatform = false;

        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollidedWithPlatform)
            return;

        if (collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _hasCollidedWithPlatform = true;
            ChangeColor();
            StartLifetimeCountdown();
        }
    }

    private void ChangeColor()
    {
        _renderer.material.color = UnityEngine.Random.ColorHSV();
    }

    private void StartLifetimeCountdown()
    {
        float lifetime = UnityEngine.Random.Range(_minLifetime, _maxLifetime);
        _lifetimeCoroutine = StartCoroutine(LifetimeCountdown(lifetime));
    }

    private IEnumerator LifetimeCountdown(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        LifetimeExpired?.Invoke(this);
    }
}
