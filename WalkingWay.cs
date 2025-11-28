using UnityEngine;

public class WalkingWay : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private float _speed = 5;

    private int _currentWeyPoint = 0;

    void Update()
    {
        if (transform.position == _wayPoints[_currentWeyPoint].position)
        {
            _currentWeyPoint = (_currentWeyPoint + 1) % _wayPoints.Length;
        }

        transform.position = Vector3.MoveTowards(transform.position, _wayPoints[_currentWeyPoint].position, _speed * Time.deltaTime);
    }
}
