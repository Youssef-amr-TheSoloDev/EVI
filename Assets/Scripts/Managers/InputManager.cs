using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public NewInputSystem Input
    {
        get
        {
            if (_allowInput) return _inputs;
            return null;
        }
    }

    public Mouse CurrentMouse
    {
        get => Mouse.current;
    }

    private bool _allowInput = true;
    private NewInputSystem _inputs;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one -InputManager- in scene");
            return;
        }
        Instance = this;
        _inputs = new();
    }

    void Start()
    {
        GameEventBus.Instance.OnInputToggle += ToggleInput;
    }

    void OnEnable() => _inputs.Enable();
    void OnDisable()
    {
        _inputs.Disable();
        GameEventBus.Instance.OnInputToggle -= ToggleInput;
    }

    public void ToggleInput() => _allowInput = !_allowInput;

}
