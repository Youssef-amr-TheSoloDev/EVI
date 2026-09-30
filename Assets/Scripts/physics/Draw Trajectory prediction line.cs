using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectoryPredictionLine : MonoBehaviour
{
    [SerializeField] private int NumOfIndicators = 8;
    [SerializeField] private GameObject IndicatorPrefab;
    [SerializeField] private Rigidbody2D _rb;
    private List<GameObject> _indicators = new();

    void Start()
    {

        for (int _ = 0; _ < NumOfIndicators; _++)
            _indicators.Add(Instantiate(IndicatorPrefab, Vector2.zero, Quaternion.identity, transform));
    }

    void FixedUpdate()
    {
        Vector2 m_lastStepPosition = _rb.position;
        Vector2 m_simVelocity = _rb.linearVelocity;

        for (int m_simStep = 0; m_simStep < NumOfIndicators; m_simStep++)
        {
            // a = f / m
            Vector2 m_gravityVel = GravityPhysicsEngine.Instance.GetGravityForcesUsingPlayerMass(m_lastStepPosition) / GravityPhysicsEngine.Instance.GetPlayerMass();

            // v1 = a + v0
            m_simVelocity += m_gravityVel * Time.deltaTime;

            // p1 = v + p0
            m_lastStepPosition += m_simVelocity;

            //* set 8 in front of the play ship
            SetIndAtPos(m_simStep, m_lastStepPosition, m_simVelocity);
        }
    }

    private void SetIndAtPos(int indIdx, Vector2 pos, Vector2 vel)
    {
        GameObject m_indicator = _indicators[indIdx];
        m_indicator.transform.position = pos;
        m_indicator.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, vel));
    }
}
