using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private const string Blend = nameof(Blend);

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _raycastOffset = 0.1f;

    private static readonly int BlendHash = Animator.StringToHash(nameof(Blend));

    private Rigidbody _rigidbody;
    private Animator _animator;

    private Vector3 _direction;
    private bool _wasOnPlatform;

    public event Action<Enemy> ReachedPoint;

    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckPlatformBelow();
        
        transform.position += _direction * _speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(_direction);
        _animator.SetFloat(BlendHash, _speed);
    }

    private void CheckPlatformBelow()
    {
        Vector3 rayStartPosition = transform.position + Vector3.up * _raycastOffset;
        Ray ray = new Ray(rayStartPosition, Vector3.down);
        
        bool isOnPlatform = false;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.TryGetComponent<Platform>(out _))
            {
                isOnPlatform = true;
            }
        }

        if (_wasOnPlatform && !isOnPlatform)
        {
            ReachedPoint?.Invoke(this);
        }

        _wasOnPlatform = isOnPlatform;
    }

    public void Initialize(Vector3 direction)
    {
        _direction = direction.normalized;
        _wasOnPlatform = false;
    }
}
