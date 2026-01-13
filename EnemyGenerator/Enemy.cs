using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _stoppingDistance = 0.1f;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private bool _hasReachedPoint;
    private Vector3 _direction;

    public event Action<Enemy> ReachedPoint;

    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        _hasReachedPoint = false;
        ReachedPoint = null;
    }

    private void Update()
    {
        if (!_hasReachedPoint)
        {
            if (Vector3.Distance(transform.position, _direction) <= _stoppingDistance)
            {
                _hasReachedPoint = true;
                ReachedPoint?.Invoke(this);
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, _direction, _speed * Time.deltaTime);
        }
        transform.rotation = Quaternion.LookRotation(_direction);
        _animator.SetFloat("Blend", _speed);
    }

    public void Initialize(Vector3 wayPoint)
    {
        _hasReachedPoint = false;
        _direction = wayPoint;
    }
}
