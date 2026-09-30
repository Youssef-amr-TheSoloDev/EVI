using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Vector2Int ZoomBounds = new(8, 16);
    [SerializeField] private float Sensitivity = 2;

    private Camera _camera;
    #region InputSystem
    private NewInputSystem _inputs;
    void Awake() => _inputs = new();
    void OnEnable() => _inputs.Enable();
    void OnDisable() => _inputs.Disable();
    #endregion

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {

        float m_scrollDelta = _inputs.Player.zoom.ReadValue<Vector2>().y;
        _camera.orthographicSize = Mathf.RoundToInt(_camera.orthographicSize - m_scrollDelta * Sensitivity);
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, ZoomBounds.x, ZoomBounds.y);
    }
}
