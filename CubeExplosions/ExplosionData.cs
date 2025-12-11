using UnityEngine;

public class ExplosionData : MonoBehaviour
{
    [SerializeField] private float _initialChance = 100f;

    private float _currentChance;

    private void Awake()
    {
        _currentChance = _initialChance;
    }

    public void InitializeFromParent(float parentChance)
    {
        _currentChance = parentChance / 2f;
    }

    public float GetCurrentChance()
    {
        return _currentChance;
    }
}
