using UnityEngine;
using UnityEngine.UI;

public class CounterClicker : MonoBehaviour
{
    [SerializeField] private CoroutineCounter _counter;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnToggleButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnToggleButtonClicked);
    }

    private void OnToggleButtonClicked()
    {
        _counter.ToggleCounting();
    }
}

