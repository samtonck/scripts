using UnityEngine;

public class ExplosionData : MonoBehaviour
{
    [SerializeField] private float _initialChance = 100f;
    [SerializeField] private float _divisionFactor = 2f;

    private float _currentChance;

    public float CurrentChance => _currentChance;

    private void Awake()
    {
        _currentChance = _initialChance;
    }

    public void InitializeFromParent(float parentChance)
    {
        _currentChance = parentChance / _divisionFactor;
    }
}
