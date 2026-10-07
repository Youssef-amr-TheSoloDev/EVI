using UnityEngine;
using System.Collections;

public class PlanetNavigation : MonoBehaviour
{
    [SerializeField] private GameObject[] Planets;
    [SerializeField] private float TransitionSpeed = 2f, DistanceToStop = 0.1f;
    [SerializeField] private UIAutoAnimation Animation;
    private Transform _t;
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

    #region InputSystem
    private NewInputSystem _inputs;
    void Awake() => _inputs = new();
    void OnEnable() => _inputs.Enable();
    void OnDisable() => _inputs.Disable();
    #endregion

    void Start()
    {
        _t = transform;
        _MapState = false;
        _inputs.Player.Map.performed += _ => _MapState = !_MapState;
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
