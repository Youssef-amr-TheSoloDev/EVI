using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;

public class PlanetNavigation : MonoBehaviour
{
    [SerializeField] private GameObject[] Planets;
    [SerializeField] private Camera MapCamera;
    [SerializeField] private float TransitionSpeed = 2f, DistanceToStop = 0.1f;
    private const string PLANET_TAG = "Planets";
    private Transform _t;
    private GameObject _objectUnderMouse;


    void Start()
    {
        _t = transform;
        InputManager.Instance.Input.Player.MouseLeftClick.performed += SelectPlanet;
    }

    void OnDisable()
    {
        InputManager.Instance.Input.Player.MouseLeftClick.performed -= SelectPlanet;
    }

    private void SelectPlanet(InputAction.CallbackContext context)
    {
        GameEventBus.Instance.OnPlanetSelect?.Invoke(_objectUnderMouse);
    }

    void Update()
    {
        _objectUnderMouse = GetObjectUnderMouse(PLANET_TAG);
        GameEventBus.Instance.OnPlanetHaver?.Invoke(_objectUnderMouse);
    }

    private GameObject GetObjectUnderMouse(string TAG)
    {
        Vector2 mouseScreenPos = InputManager.Instance.CurrentMouse.position.value;
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
            if (hit.collider.CompareTag(TAG))
                return hit.collider.gameObject;
        }

        return null;
    }

    public void StopAllMovements()
    {
        StopAllCoroutines();
    }

    public void GoToPlanet(int idx)
    {
        StopAllCoroutines();
        StartCoroutine(GoToPlanetCor(idx));
    }

    private IEnumerator GoToPlanetCor(int idx)
    {
        float m_zOffset = _t.position.z;
        yield return new WaitUntil(() =>
        {
            _t.position = Vector3.Lerp(_t.position, Planets[idx].transform.position + new Vector3(0, 0, m_zOffset), Time.deltaTime * TransitionSpeed);
            return Vector2.Distance(_t.position, Planets[idx].transform.position) < DistanceToStop;
        });
    }
}
