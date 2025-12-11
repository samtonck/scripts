using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public event Action OnLeftMouseClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseClick?.Invoke();
        }
    }
}
