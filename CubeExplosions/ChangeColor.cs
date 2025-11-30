using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public void Change()
    {
        Renderer renderer = GetComponent<Renderer>();
        
        Color randomColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        
        renderer.material.color = randomColor;
    }
}

