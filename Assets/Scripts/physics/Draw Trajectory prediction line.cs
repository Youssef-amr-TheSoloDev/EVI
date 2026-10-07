using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectoryPredictionLine : MonoBehaviour
{
    [Header("Prediction")]
    [Tooltip("How many seconds of future flight to simulate.")]
    [SerializeField, Min(0.1f)] private float SimTimeAhead = 2f;
    [Tooltip("Fixed number of dots spread evenly over SimTimeAhead.")]
    [SerializeField, Min(2)] private int NumOfIndicators = 10;
    [SerializeField] private GameObject IndicatorPrefab;
    [SerializeField] private Rigidbody2D _rb;

    [Header("Collision (world radius for impact check)")]
    [Tooltip("Radius used for planet-impact check. ~0.2 matches engine queries.")]
    [SerializeField] private float ShipRadius = 0.2f;
    [Tooltip("Extra skin so the line stops just before visually touching a planet.")]
    [SerializeField] private float ImpactSkin = 0.02f;

    private readonly List<GameObject> _indicators = new();
    private readonly List<PlanetAtmosphere> _atmospheres = new();
    private readonly List<Collider2D> _planetSolids = new();

    void Awake()
    {
        if (_rb == null)
            _rb = GetComponentInParent<Rigidbody2D>();
        RefreshCaches();
    }

    private void RefreshCaches()
    {
        _atmospheres.Clear();
#if UNITY_2023_1_OR_NEWER
        foreach (PlanetAtmosphere m_atmo in FindObjectsByType<PlanetAtmosphere>(FindObjectsSortMode.None))
#else
        foreach (PlanetAtmosphere m_atmo in FindObjectsOfType<PlanetAtmosphere>())
#endif
        {
            if (m_atmo != null) _atmospheres.Add(m_atmo);
        }

        _planetSolids.Clear();
        foreach (GameObject m_planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            if (m_planet == null) continue;
            foreach (Collider2D m_col in m_planet.GetComponentsInChildren<Collider2D>())
            {
                if (m_col != null && !m_col.isTrigger)
                    _planetSolids.Add(m_col);
            }
        }
    }

    void Start()
    {
        RefreshCaches();
        RebuildIndicators();
    }

    void OnValidate()
    {
        if (SimTimeAhead < 0.1f) SimTimeAhead = 0.1f;
        if (NumOfIndicators < 2) NumOfIndicators = 2;
        if (ShipRadius < 0.01f) ShipRadius = 0.01f;
        if (Application.isPlaying && _indicators.Count != NumOfIndicators)
            RebuildIndicators();
    }

    private void RebuildIndicators()
    {
        for (int m_idx = 0; m_idx < _indicators.Count; m_idx++)
        {
            if (_indicators[m_idx] != null)
                Destroy(_indicators[m_idx]);
        }
        _indicators.Clear();

        if (IndicatorPrefab == null) return;
        for (int m_idx = 0; m_idx < NumOfIndicators; m_idx++)
        {
            GameObject m_dot = Instantiate(IndicatorPrefab, Vector2.zero, Quaternion.identity, transform);
            m_dot.SetActive(false);
            _indicators.Add(m_dot);
        }
    }

    void FixedUpdate()
    {
        if (_rb == null || IndicatorPrefab == null) return;
        if (GravityPhysicsEngine.Instance == null) return;
        if (_indicators.Count != NumOfIndicators) RebuildIndicators();

        float m_dt = Time.fixedDeltaTime;
        int m_totalSteps = Mathf.Max(Mathf.RoundToInt(SimTimeAhead / m_dt), NumOfIndicators);
        float m_stepsPerDot = (float)m_totalSteps / NumOfIndicators;
        float m_nextDotAt = m_stepsPerDot;
        int m_placedDots = 0;

        Vector2 m_pos = _rb.position;
        Vector2 m_vel = _rb.linearVelocity;

        for (int m_step = 1; m_step <= m_totalSteps; m_step++)
        {
            // 1. gravity (effective accel at SIMULATED pos, not player pos)
            Vector2 m_accel = GravityPhysicsEngine.Instance.GetGameplayGravityAccelerationAtPosition(m_pos);

            // 2. semi-implicit Euler velocity update
            m_vel += m_accel * m_dt;

            // 3. atmospheric drag (same scaling + order as live game)
            for (int m_atmoIdx = 0; m_atmoIdx < _atmospheres.Count; m_atmoIdx++)
            {
                PlanetAtmosphere m_atmo = _atmospheres[m_atmoIdx];
                if (m_atmo == null) continue;
                if (IsPointInsideTrigger(m_atmo, m_pos))
                {
                    m_vel = m_atmo.ApplyAtmosphericDrag(m_vel, m_dt);
                    break;
                }
            }

            // 4. position update
            m_pos += m_vel * m_dt;

            // 5. planet impact: place one dot at impact, hide the rest
            if (HitsPlanet(m_pos))
            {
                if (m_placedDots < NumOfIndicators)
                {
                    SetIndAtPos(m_placedDots, m_pos, m_vel, true);
                    m_placedDots++;
                }
                for (int m_hide = m_placedDots; m_hide < NumOfIndicators; m_hide++)
                    _indicators[m_hide].SetActive(false);
                return;
            }

            // 6. drop a dot each time we cross the next sample boundary
            if (m_step >= m_nextDotAt - 0.0001f && m_placedDots < NumOfIndicators)
            {
                SetIndAtPos(m_placedDots, m_pos, m_vel, true);
                m_placedDots++;
                m_nextDotAt += m_stepsPerDot;
            }
        }

        // Safety: hide any dots never reached (rounding leftovers).
        for (int m_hide = m_placedDots; m_hide < NumOfIndicators; m_hide++)
            _indicators[m_hide].SetActive(false);
    }

    private bool IsPointInsideTrigger(PlanetAtmosphere atmo, Vector2 point)
    {
        Collider2D m_col = atmo.GetComponent<Collider2D>();
        if (m_col == null || !m_col.enabled) return false;
        return m_col.OverlapPoint(point);
    }

    private bool HitsPlanet(Vector2 point)
    {
        for (int m_idx = 0; m_idx < _planetSolids.Count; m_idx++)
        {
            Collider2D m_solid = _planetSolids[m_idx];
            if (m_solid == null || !m_solid.enabled) continue;
            Vector2 m_closest = m_solid.ClosestPoint(point);
            if (Vector2.Distance(m_closest, point) <= ShipRadius + ImpactSkin)
                return true;
        }
        return false;
    }

    private void SetIndAtPos(int indIdx, Vector2 pos, Vector2 vel, bool visible)
    {
        GameObject m_indicator = _indicators[indIdx];
        m_indicator.SetActive(visible);
        if (!visible) return;
        m_indicator.transform.position = pos;
        if (vel.sqrMagnitude > 0.000001f)
            m_indicator.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, vel));
    }
}
