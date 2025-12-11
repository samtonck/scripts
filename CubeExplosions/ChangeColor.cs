using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Change()
    {
        Color randomColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        
        _renderer.material.color = randomColor;
    }
}

