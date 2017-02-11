using UnityEngine;
using jaburns.UnityTools;

public class Elastoball : MonoBehaviour
{
    [SerializeField] GameObject _elasticPrefab;
    [SerializeField] float _impulseMultiplier = 1f;
    [SerializeField] Rigidbody _realBall;

    GameObject _elastic;
    GameObject _pinchAnchor;

    Transform _realBallMesh;

    bool _enabled;

    void Start()
    {
        var pinchTarget = GetComponent<HandPinchTriggerTarget>();
        pinchTarget.PinchStart.Sub(gameObject, OnPinchStart);
        pinchTarget.PinchEnd.Sub(gameObject, OnPinchEnd);

        _realBallMesh = _realBall.GetComponentInChildren<MeshRenderer>().transform;
        _realBall.velocity = 5*Vector3.forward + 2*Vector3.right;

        Time.timeScale = .1f;
    }

    void Update()
    {
        var vXY = _realBall.velocity.XZ();
        var axis = vXY.Rotated(-90).AsVector3WithY(0);
        var angVel = vXY.magnitude / 0.25f;
        var deltaAngle = angVel * Time.deltaTime * Mathf.Rad2Deg;

        _realBallMesh.Rotate(axis, deltaAngle);

        transform.position = _realBall.transform.position.WithY(1.5f);
        _enabled = _realBall.velocity.sqrMagnitude < .1f;

        GetComponentInChildren<Renderer>().enabled = _enabled;

        if (_elastic != null) {
            var ds = _pinchAnchor.transform.position - transform.position;
            _elastic.transform.rotation = Quaternion.LookRotation(ds);
            _elastic.transform.localScale = new Vector3(1, 1, ds.magnitude);
        }
    }

    void OnPinchStart(GameObject anchor)
    {
        if (!_enabled || _elastic != null) return;

        _pinchAnchor = anchor;
        _elastic = Instantiate(_elasticPrefab, transform.position, Quaternion.identity);
    }

    void OnPinchEnd()
    {
        if (!_enabled || _elastic == null) return;

        var ds = _pinchAnchor.transform.position - transform.position;
        var impulse = -_impulseMultiplier * ds;

        if (impulse.y < 0) impulse = impulse.WithY(0);

        _realBall.AddForce(impulse, ForceMode.VelocityChange);

        Destroy(_elastic);
        _elastic = null;
        _pinchAnchor = null;
    }
}
