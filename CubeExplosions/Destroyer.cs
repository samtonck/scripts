using UnityEngine;

[RequireComponent(typeof(Spawner), typeof(Explosion))]
public class Destroyer : MonoBehaviour
{
    private Spawner _spawner;
    private Explosion _explosion;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
        _explosion = GetComponent<Explosion>();
    }

    private void OnEnable()
    {
        _spawner.ObjectDestroyed += DestroyObject;
        _explosion.ObjectExploded += DestroyObject;
    }

    private void OnDisable()
    {
        _spawner.ObjectDestroyed -= DestroyObject;
        _explosion.ObjectExploded -= DestroyObject;
    }

    private void DestroyObject(GameObject targetObject)
    {
        Destroy(targetObject);
    }
}

