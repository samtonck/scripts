using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Chiken : MonoBehaviour
{
    private const string Blend = nameof(Blend);

    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _reachRadius = 0.1f;

    private static readonly int BlendHash = Animator.StringToHash(nameof(Blend));

    private Rigidbody _rigidbody;
    private Animator _animator;

    public Rigidbody Rigidbody => _rigidbody;

    private int _currentWeyPoint = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 targetPosition = _wayPoints[_currentWeyPoint].position;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= _reachRadius)
        {
            _currentWeyPoint = (_currentWeyPoint + 1) % _wayPoints.Length;
            targetPosition = _wayPoints[_currentWeyPoint].position;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        
        if (direction != Vector3.zero)
        {
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

        _animator.SetFloat(BlendHash, _speed);
    }

    public void Initialize(Transform[] wayPoints)
    {
        _wayPoints = wayPoints;
    }
}
