using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitDrawer : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private bool UpdateOrbitLocation;
    private LineRenderer _line;
    private Transform _t;

    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _t = transform;
    }


    void Update()
    {
        if (!UpdateOrbitLocation) return;
        DrawOrbit();
    }

    [ContextMenu("Draw Orbit")]
    void DrawOrbit()
    {
        _line = _line != null ? _line : GetComponent<LineRenderer>(); _line.positionCount = 0;
        _t = _t != null ? _t : transform;


        float m_distanceToTarget = Vector2.Distance(_t.position, Target.position);
        float m_step = 500f / m_distanceToTarget;

        float m_currentAngle = 0;
        while (m_currentAngle < 360)
        {
            DrawPointOnLine(Quaternion.Euler(0, 0, m_currentAngle) * (_t.position - Target.position) + Target.position);
            m_currentAngle += m_step;
        }
    }

    private void DrawPointOnLine(Vector2 pos)
    {
        _line.positionCount++;
        _line.SetPosition(_line.positionCount - 1, pos);
    }
}
