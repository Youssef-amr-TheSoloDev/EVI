using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;

public class GravityPhysicsEngine : MonoBehaviour
{
    public static GravityPhysicsEngine Instance { get; private set; }

    [SerializeField] private Rigidbody2D Player;
    [SerializeField] private float G = 0.001f, EffectDistance = 0.2f;

    private GameObject[] Planets;
    private GameObject _sun;
    void Awake()
    {
        Planets = GameObject.FindGameObjectsWithTag("Planet");
        _sun = Planets.First(planet => planet.name == "Sun");

        if (Instance == null)
        {
            Instance = this;
            return;
        }
        print("there is more then 1 Physics managers");
    }

    void Update()
    {
        ApplyGravityToPlayer(GetGravityForcesInWorld(Player));
    }

    public Vector2 GetGravityForcesInWorld(Rigidbody2D playerRb)
    {
        Vector2 m_forces = new();
        for (int m_planetIdx = 0; m_planetIdx < Planets.Length; m_planetIdx++)
        {
            // if ship isn't in the SOI of the planet, don't apply it's gravity
            if (Vector2.Distance(Planets[m_planetIdx].transform.position, playerRb.position) > EffectDistance * Planets[m_planetIdx].GetComponent<Rigidbody2D>().mass)
                continue;

            // calculate the force in the direction of the current planet
            m_forces += GetGravityForceWithABody(Planets[m_planetIdx].GetComponent<Rigidbody2D>(), playerRb);
        }

        if (m_forces == Vector2.zero)
        {
            // if not in the SOI of any planet apply sun gravity
            // 0 idx is the sun
            return GetGravityForceWithABody(_sun.GetComponent<Rigidbody2D>(), Player);
        }

        return m_forces;
    }

    private Vector2 GetGravityForceWithABody(Rigidbody2D body1, Rigidbody2D body2)
    {
        return G * body2.mass * body1.mass * Mathf.Pow(Vector2.Distance(body1.transform.position, body2.position), 2f) * (body1.position - body2.position).normalized;
    }

    public Vector2 GetGravityForcesOnPlayer()
    {
        return GetGravityForcesInWorld(Player);
    }

    public Vector2 GetGravityForcesUsingPlayerMass(Vector2 _currentPos)
    {
        return GetGravityForcesInWorld(Player);
    }
    public Vector2 GetGravityForcesDirectionVector()
    {
        return GetGravityForcesInWorld(Player).normalized;
    }

    public float GetGravityForcesMag()
    {
        return GetGravityForcesInWorld(Player).magnitude;
    }

    public float GetGravityForcesAngles()
    {
        return Vector2.SignedAngle(Vector2.right, GetGravityForcesDirectionVector());
    }

    public float GetPlayerMass()
    {
        return Player.mass;
    }

    private void ApplyGravityToPlayer(Vector2 m_SumOfGravityForces)
    {
        Player.AddForce(Time.deltaTime * m_SumOfGravityForces, ForceMode2D.Force);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (Planets == null) return;

        for (int idx = 0; idx < Planets.Length; idx++)
        {
            float m_DistanceFromPlanet = Vector2.Distance(Planets[idx].transform.position, Player.position);
            if (m_DistanceFromPlanet <= EffectDistance * Planets[idx].GetComponent<Rigidbody2D>().mass)
            {
                Gizmos.DrawWireSphere(Planets[idx].transform.position, EffectDistance * Planets[idx].GetComponent<Rigidbody2D>().mass);
            }

        }
    }
}
