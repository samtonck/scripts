using System;
using System.Collections;
using UnityEngine;

public class CoroutineCounter : MonoBehaviour
{
    private const float CountInterval = 0.5f;
    private const float Increment = 0.5f;

    private Coroutine _countingCoroutine;
    private bool _isCounting = false;

    public event Action<float> ValueChanged;

    public float CurrentValue { get; private set; }
    
    public void ToggleCounting()
    {
        if (_isCounting)
        {
            StopCounting();
        }
        else
        {
            StartCounting();
        }
    }

    private void StartCounting()
    {
        if (_countingCoroutine == null)
        {
            _isCounting = true;
            _countingCoroutine = StartCoroutine(CountingProcess());
        }
    }

    private void StopCounting()
    {
        if (_countingCoroutine != null)
        {
            _isCounting = false;
            StopCoroutine(_countingCoroutine);
            _countingCoroutine = null;
        }
    }

    private IEnumerator CountingProcess()
    {
        WaitForSeconds wait = new WaitForSeconds(CountInterval);

        while (_isCounting)
        {
            yield return wait;
            CurrentValue += Increment;
            ValueChanged?.Invoke(CurrentValue);
        }
    }

    private void OnDisable()
    {
        StopCounting();
    }
}
