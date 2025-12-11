using System;
using UnityEngine;

public class Handler : MonoBehaviour
{
    public event Action<GameObject> OnExplode;
    public event Action<GameObject> OnDivide;

    private void OnEnable()
    {
        var raycaster = GetComponent<Raycaster>();
        raycaster.OnObjectHit += HandleClick;
    }

    private void OnDisable()
    {
        var raycaster = GetComponent<Raycaster>();
        raycaster.OnObjectHit -= HandleClick;
    }

    private bool CanDivide(float currentChance)
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        return randomValue < currentChance;
    }

    private void HandleClick(GameObject hitObject)
    {
        ExplosionData explosionData = hitObject.GetComponent<ExplosionData>();
        if (explosionData == null) return;

        if (CanDivide(explosionData.GetCurrentChance()))
        {
            OnDivide?.Invoke(hitObject);
            Debug.Log($"Деление {explosionData.GetCurrentChance()}");
        }
        else
        {
            OnExplode?.Invoke(hitObject);
            Debug.Log($"Взрыв {explosionData.GetCurrentChance()}");
        }
    }
}
