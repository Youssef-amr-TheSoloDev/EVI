using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MapController : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] private Camera MapCamera;
    [SerializeField] private Transform MapCameraTransform;
    [SerializeField] private float DeltaSpeed = 2f, DeltaSensitivity = 2f;
    [SerializeField] private Material MapMat;
    [SerializeField] private PlanetNavigation navigation;
    private Vector2 _MaterialOffset;
    private GameObject _objectUnderMouse;
    public void OnDrag(PointerEventData eventData)
    {
        navigation.StopAllMovements();
        MapCameraTransform.transform.position = Vector3.Lerp(MapCameraTransform.transform.position, MapCameraTransform.position - (Vector3)eventData.delta * (DeltaSensitivity + (MapCamera.orthographicSize / 20)), Time.deltaTime * DeltaSpeed);

        _MaterialOffset -= eventData.delta * (DeltaSensitivity + (MapCamera.orthographicSize / 20));
        MapMat.SetVector("offset", _MaterialOffset);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.value;
        Vector3 mouseWorldPos = MapCamera.ScreenToWorldPoint(
            new Vector3(
                mouseScreenPos.x,
                mouseScreenPos.y,
                -MapCamera.transform.position.z
            )
        );

        RaycastHit2D hit = Physics2D.CircleCast(
            mouseWorldPos,
            1f,
            Vector2.right
        );

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Planet"))
                _objectUnderMouse = hit.collider.gameObject;
        }
    }
}
