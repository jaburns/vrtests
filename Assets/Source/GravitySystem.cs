using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    [SerializeField] float _minForce = 0.5f;
    [SerializeField] float _gravConstant = 10;
    [SerializeField] float _distanceOffset = 2;
    [SerializeField] GameObject _planetPrefab;
    [SerializeField] GameObject _planetSpawnPoint;

    GravitationalBody[] _bodies;

    void Start ()
    {
        FindBodies();
    }
    
    void FixedUpdate()
    {
        for (int i = 0; i < _bodies.Length - 1; ++i) {
            for (int j = i+1; j < _bodies.Length; ++j) {
                var a = _bodies[i].transform.position;
                var b = _bodies[j].transform.position;
                var aToB = b - a;

                var d = aToB.magnitude + _distanceOffset;
                var F = _bodies[i].mass * _bodies[j].mass * _gravConstant / (d*d);
                if (F < _minForce) F = _minForce;

                var aToBUnit = aToB.normalized;
                if (_bodies[i].rb) _bodies[i].rb.AddForce( aToBUnit * F);
                if (_bodies[j].rb) _bodies[j].rb.AddForce(-aToBUnit * F);
            }
        }
    }

    public void FindBodies()
    {
        _bodies = FindObjectsOfType<GravitationalBody>();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) {
            Instantiate(_planetPrefab, _planetSpawnPoint.transform.position, Quaternion.identity);
            FindBodies();
        }
    }
}
