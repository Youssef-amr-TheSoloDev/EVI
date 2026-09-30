using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlanetAtmosphere : MonoBehaviour
{
    [SerializeField] private float Drag;
    private CircleCollider2D _atmosphere;
    private List<Collider2D> _bodiesInAtmosphere = new();

    void Start()
    {
        _atmosphere = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        _atmosphere.Overlap(_bodiesInAtmosphere);
        if (_bodiesInAtmosphere.Count == 0) return;

        foreach (Collider2D _body in _bodiesInAtmosphere)
        {
            Rigidbody2D _bodyRigidBody = _body.GetComponent<Rigidbody2D>();
            _bodyRigidBody.linearVelocity *= Drag;
        }
    }
}
