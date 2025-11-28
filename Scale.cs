using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField] private float _minSize = 1;
    [SerializeField] private float _maxSize = 2;
    [SerializeField] private float _speedScale = 1;

    private bool _isIncrease = true;

    void Update()
    {
        Vector3 objectScale = transform.localScale;
        if (_isIncrease)
        {
            transform.localScale = new Vector3(objectScale.x + (_speedScale * Time.deltaTime), objectScale.y + (_speedScale * Time.deltaTime), objectScale.z + (_speedScale * Time.deltaTime));
            if (objectScale.x > _maxSize) _isIncrease = false;
        }
        else
        {
            transform.localScale = new Vector3(objectScale.x - (_speedScale * Time.deltaTime), objectScale.y - (_speedScale * Time.deltaTime), objectScale.z - (_speedScale * Time.deltaTime));
            if (objectScale.x < _minSize) _isIncrease = true;
        }
    }
}
