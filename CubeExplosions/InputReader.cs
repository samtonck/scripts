using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const string PrimaryInteractionButton = "Fire1";

    public event Action InteractionPressed;

    private void Update()
    {
        if (Input.GetButtonDown(PrimaryInteractionButton))
        {
            InteractionPressed?.Invoke();
            Debug.Log($"Клик");
        }
    }
}
