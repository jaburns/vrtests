using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    Dictionary<PlanetController, int> _counters;

    public GravitationalBody gravBody { get; private set; }

    bool _mergeEnabled;

    void Awake()
    {
        _counters = new Dictionary<PlanetController, int>();
        _mergeEnabled = false;
        gravBody = GetComponent<GravitationalBody>();
    }

    public void DisableMerge()
    {
        _mergeEnabled = false;
    }

    void OnCollisionEnter(Collision c)
    {
        if (!_mergeEnabled) return;

        var planet = c.gameObject.GetComponent<PlanetController>();
        if (planet != null) {
            _counters[planet] = 0;
        }
    }

    void OnCollisionStay(Collision c)
    {
        if (!_mergeEnabled) return;

        var planet = c.gameObject.GetComponent<PlanetController>();
        if (planet != null) {
            var count = ++_counters[planet];

            if (count >= 1) {
                planet.DisableMerge();
                gravBody.IncreaseMass(planet.gravBody.mass);
                Destroy(planet.gameObject);

                FindObjectOfType<GravitySystem>().FindBodies();
            }
        }
    }
}
