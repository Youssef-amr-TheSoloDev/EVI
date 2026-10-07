using UnityEngine;
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

    void FixedUpdate()
    {
        if (Player == null) return;
        // Null-safe: CircleCast returns no collider in deep space.
        RaycastHit2D m_hit = Physics2D.CircleCast(Player.position, 0.2f, Vector2.zero);
        if (m_hit.collider != null && m_hit.collider.CompareTag("Planet"))
            return;
        ApplyGravityToPlayer(GetGravityForcesInWorld(Player));
    }

    public Vector2 GetGameplayGravityAccelerationAtPosition(Vector2 simPos)
    {
        float m_mass = GetPlayerMass();
        if (m_mass <= 0f) return Vector2.zero;
        return GetGravityForceAtPosition(simPos, m_mass) / m_mass * Time.fixedDeltaTime;
    }

    public Vector2 GetGravityForceAtPosition(Vector2 position, float mass)
    {
        if (Planets == null) return Vector2.zero;
        Vector2 m_forces = new();
        for (int m_planetIdx = 0; m_planetIdx < Planets.Length; m_planetIdx++)
        {
            GameObject m_planet = Planets[m_planetIdx];
            if (m_planet == null) continue;
            Rigidbody2D m_planetRb = m_planet.GetComponent<Rigidbody2D>();
            if (m_planetRb == null) continue;

            // if ship isn't in the SOI of the planet, don't apply its gravity
            if (Vector2.Distance(m_planet.transform.position, position) > EffectDistance * m_planetRb.mass)
                continue;

            m_forces += GetGravityForceBetween(m_planetRb, position, mass);
        }

        if (m_forces == Vector2.zero)
        {
            // if not in the SOI of any planet apply sun gravity (infinite range fallback)
            if (_sun != null)
            {
                Rigidbody2D m_sunRb = _sun.GetComponent<Rigidbody2D>();
                if (m_sunRb != null)
                    return GetGravityForceBetween(m_sunRb, position, mass);
            }
        }

        return m_forces;
    }

    private Vector2 GetGravityForceBetween(Rigidbody2D planetRb, Vector2 shipPos, float shipMass)
    {
        Vector2 m_planetPos = planetRb.transform.position;
        float m_dist = Vector2.Distance(m_planetPos, shipPos);
        if (m_dist < 0.001f) return Vector2.zero;
        Vector2 m_dir = (m_planetPos - shipPos) / m_dist;
        return G * shipMass * planetRb.mass / (m_dist * m_dist) * m_dir;
    }

    public Vector2 GetGravityForcesInWorld(Rigidbody2D playerRb)
    {
        return GetGravityForceAtPosition(playerRb.position, playerRb.mass);
    }

    private Vector2 GetGravityForceWithABody(Rigidbody2D body1, Rigidbody2D body2)
    {
        return G * body2.mass * body1.mass / Mathf.Pow(Vector2.Distance(body1.transform.position, body2.position), 2f) * (body1.position - body2.position).normalized;
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
        if (Player && _sun)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Player.position, GetGravityForcesInWorld(Player) + Player.position);
        }

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
