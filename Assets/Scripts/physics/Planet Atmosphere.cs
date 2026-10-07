using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlanetAtmosphere : MonoBehaviour
{
    public float Drag;
    private CircleCollider2D _atmosphere;
    private List<Collider2D> _bodiesInAtmosphere = new();

    void Awake()
    {
        _atmosphere = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        if (_atmosphere == null) _atmosphere = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate()
    {
        if (_atmosphere == null) return;
        _atmosphere.Overlap(_bodiesInAtmosphere);
        if (_bodiesInAtmosphere.Count == 0) return;

        foreach (Collider2D _body in _bodiesInAtmosphere)
        {
            if (_body == null) continue;
            Rigidbody2D _bodyRigidBody = _body.attachedRigidbody;
            if (_bodyRigidBody == null) continue;
            _bodyRigidBody.linearVelocity = ApplyAtmosphericDrag(_bodyRigidBody.linearVelocity, Time.fixedDeltaTime);
        }
    }

    public Vector2 ApplyAtmosphericDrag(Vector2 incoming)
    {
        return ApplyAtmosphericDrag(incoming, Time.deltaTime);
    }

    public Vector2 ApplyAtmosphericDrag(Vector2 incoming, float dt)
    {
        float m_factor = 1f - Drag * dt;
        if (m_factor < 0f) m_factor = 0f;
        return incoming * m_factor;
    }
}
