using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class AdvancedEnemy : MonoBehaviour
{
    private const string Blend = nameof(Blend);

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _reachDistance = 1f;

    private static readonly int BlendHash = Animator.StringToHash(nameof(Blend));

    private Rigidbody _rigidbody;
    private Animator _animator;

    private Transform _target;
    private Vector3 _direction;

    public event Action<AdvancedEnemy> AdvancedEnemyReachedPoint;

    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_target == null)
            return;

        CheckTargetReached();
        UpdateDirection();
        MoveTowardsTarget();
    }

    private void UpdateDirection()
    {
        Vector3 targetPosition = _target.position;
        targetPosition.y = transform.position.y;
        _direction = (targetPosition - transform.position).normalized;
    }

    private void MoveTowardsTarget()
    {
        transform.position += _direction * _speed * Time.deltaTime;
        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_direction);
        }
        _animator.SetFloat(BlendHash, _speed);
    }

    private void CheckTargetReached()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget <= _reachDistance)
        {
            AdvancedEnemyReachedPoint?.Invoke(this);
        }
    }

    public void Initialize(Transform target)
    {
        _target = target;
    }
}
