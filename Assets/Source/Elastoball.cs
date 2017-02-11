using UnityEngine;

public class Elastoball : MonoBehaviour
{
    [SerializeField] GameObject _elasticPrefab;
    [SerializeField] float _impulseMultiplier = 1f;
    [SerializeField] Rigidbody _realBall;

    GameObject _elastic;
    GameObject _pinchAnchor;

    bool _enabled;

    void Start()
    {
        var pinchTarget = GetComponent<HandPinchTriggerTarget>();
        pinchTarget.PinchStart.Sub(gameObject, OnPinchStart);
        pinchTarget.PinchEnd.Sub(gameObject, OnPinchEnd);
    }

    void Update()
    {
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