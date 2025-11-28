using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float _speed = 45;

    private void Update()
    {
        transform.Rotate(Vector3.up * _speed * Time.deltaTime);
    }
}
