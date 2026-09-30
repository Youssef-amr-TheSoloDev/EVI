using UnityEngine;

public class PlanetOrbiter : MonoBehaviour
{
    [SerializeField] private float OrbitSpeed;
    [SerializeField] private Transform MainBodyToOrbit;
    private float _distanceFromMainBody, _currentOrbitAngle;
    private Transform _t;

    void Start()
    {
        _t = transform;
        _distanceFromMainBody = Vector2.Distance(MainBodyToOrbit.position, _t.position);
        _currentOrbitAngle = Vector2.SignedAngle(Vector2.right, (_t.position - MainBodyToOrbit.position).normalized);
    }

    void Update()
    {
        _currentOrbitAngle += 1f * OrbitSpeed * (1 / _distanceFromMainBody) * Time.deltaTime;
        _t.position = GetWorldPositionFromAngle(_currentOrbitAngle);
    }

    private Vector3 GetWorldPositionFromAngle(float OrbitAngle)
    {
        return new Vector3(
            Mathf.Cos(OrbitAngle),
            Mathf.Sin(OrbitAngle),
            0f
        ) * Vector2.Distance(MainBodyToOrbit.position, transform.position) + MainBodyToOrbit.position;
    }

    [ContextMenu("Set random Orbit Position")]
    private void SetRandomOrbitPosition()
    {
        transform.position = GetWorldPositionFromAngle(Random.value * 360f);
    }
}
