using UnityEngine;
using UnityEngine.EventSystems;


public class MapController : MonoBehaviour, IDragHandler
{
    [SerializeField] private Camera MapCamera;
    [SerializeField] private Transform MapCameraTransform;
    [SerializeField] private float DeltaSpeed = 2f, DeltaSensitivity = 2f;
    [SerializeField] private Material MapMat;
    [SerializeField] private UIAutoAnimation Animation;
    [SerializeField] private PlanetNavigation navigation;
    private Vector2 _MaterialOffset;

    private bool _MapState
    {
        get => _mapState;
        set
        {
            _mapState = value;
            if (_mapState)
                Animation.EntranceAnimation();
            else
                Animation.ExitAnimation();
        }
    }

    private bool _mapState;


    void Start()
    {
        _MapState = false;
        InputManager.Instance.Input.Player.Map.performed += ToggleMap;
    }

    void OnDisable()
    {
        InputManager.Instance.Input.Player.Map.performed -= ToggleMap;
    }

    private void ToggleMap(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _MapState = !_MapState;
    }

    public void OnDrag(PointerEventData eventData)
    {
        navigation.StopAllMovements();
        MapCameraTransform.transform.position = Vector3.Lerp(MapCameraTransform.transform.position, MapCameraTransform.position - (Vector3)eventData.delta * (DeltaSensitivity + (MapCamera.orthographicSize / 20)), Time.deltaTime * DeltaSpeed);

        _MaterialOffset -= eventData.delta * (DeltaSensitivity + (MapCamera.orthographicSize / 20));
        MapMat.SetVector("offset", _MaterialOffset);
    }
}
