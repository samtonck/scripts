using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CounterView : MonoBehaviour
{
    [SerializeField] private CoroutineCounter _counter;
    [SerializeField] private Text _counterText;

    private Coroutine _coroutine;

    private void Start()
    {
        _counterText.text = _counter.CurrentValue.ToString("F0");
    }

    private void OnEnable()
    {
        _counter.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _counter.ValueChanged -= OnValueChanged;
    }

    public void Stop()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void OnValueChanged(float newValue)
    {
        _coroutine = StartCoroutine(UpdateTextSmooth(newValue));
    }

    private IEnumerator UpdateTextSmooth(float targetValue)
    {
        _counterText.text = targetValue.ToString("F0");
        Debug.Log($"Счетчик: {targetValue}");
        yield return null;
    }
}

