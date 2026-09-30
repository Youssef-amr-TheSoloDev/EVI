using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] private Camera MapCamera;
    [SerializeField] private Transform MapCameraTransform;
    [SerializeField] private float DeltaSpeed = 2f, DeltaSensitivity = 2f;
    [SerializeField] private Material MapMat;
    [SerializeField] private PlanetNavigation navigation;
    private Vector2 _MaterialOffset;
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
}
