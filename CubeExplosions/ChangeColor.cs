using UnityEngine;

[RequireComponent(typeof(Spawner))]
public class ChangeColor : MonoBehaviour
{
    private Spawner _spawner;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
    }

    private void OnEnable()
    {
        _spawner.ObjectCloned += Change;
    }

    private void OnDisable()
    {
        _spawner.ObjectCloned -= Change;
    }

    private void Change(GameObject targetObject)
    {
        if (!targetObject.TryGetComponent(out Renderer renderer))
            return;

        Color randomColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        
        renderer.material.color = randomColor;
    }
}

