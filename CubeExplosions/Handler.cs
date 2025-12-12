using System;
using UnityEngine;

[RequireComponent(typeof(Raycaster))]
public class Handler : MonoBehaviour
{
    private Raycaster _raycaster;

    public event Action<GameObject> SpawnClones;
    public event Action<GameObject> Exploded;

    private void Awake()
    {
        _raycaster = GetComponent<Raycaster>();
    }

    private void OnEnable()
    {
        _raycaster.ObjectHit += HandleClick;
    }

    private void OnDisable()
    {
        _raycaster.ObjectHit -= HandleClick;
    }

    private bool CanDivide(float currentChance)
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        return randomValue < currentChance;
    }

    private void HandleClick(GameObject hitObject)
    {
        if (!hitObject.TryGetComponent(out ExplosionData explosionData))
            return;

        if (CanDivide(explosionData.CurrentChance))
        {
            SpawnClones?.Invoke(hitObject);
            Debug.Log($"Клонирование | шанс: {explosionData.CurrentChance}");
        }
        else
        {
            Exploded?.Invoke(hitObject);
            Debug.Log($"Взрыв | шанс: {explosionData.CurrentChance}");
        }
    }
}
