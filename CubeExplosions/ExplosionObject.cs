using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    [SerializeField] private float _initialChance = 100f;
    [SerializeField] private float _divisionFactor = 2f;

    private float _currentChance;

    public float CurrentChance => _currentChance;

    private void Awake()
    {
        _currentChance = _initialChance;
    }

    public void Initialize(Vector3 position, Vector3 scale, float parentChance)
    {
        transform.position = position;
        transform.rotation = UnityEngine.Random.rotation;
        transform.localScale = scale / 2f;
        
        InitializeFromParent(parentChance);
        
        if (TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = Random.ColorHSV();
        }
        
        if (TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.useGravity = true;
        }
    }

    public void InitializeFromParent(float parentChance)
    {
        _currentChance = parentChance / _divisionFactor;
    }
}

