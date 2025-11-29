using UnityEngine;
using UnityEngine.UI;

public class CounterView : MonoBehaviour
{
    [SerializeField] private CoroutineCounter _counter;
    [SerializeField] private Text _counterText;

    private void Start()
    {
        _counterText.text = _counter.CurrentValue.ToString("F1");
    }

    private void OnEnable()
    {
        _counter.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _counter.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(float newValue)
    {
        _counterText.text = newValue.ToString("F1");
        Debug.Log($"Счетчик: {newValue}");
    }
}

