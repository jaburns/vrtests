using UnityEngine;
using jaburns.UnityTools;

public class SlingBall : MonoBehaviour
{
    [SerializeField] float _impulseMultiplier = 20f;
    [SerializeField] GameObject _elasticPrefab;
    [SerializeField] PredictionLine _predictionLine;

    Rigidbody _rb;
    GameObject _pinchAnchor;
    GameObject _elastic;

//  Vector3 _position;
//  Vector3 _velocity;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
//      _position = transform.position;
    }

    void Start()
    {
        var pinchTarget = GetComponentInChildren<HandPinchTriggerTimeless>();
        pinchTarget.PinchStart.Sub(gameObject, OnPinchStart);
        pinchTarget.PinchEnd.Sub(gameObject, OnPinchEnd);
    }

//  void FixedUpdate()
//  {
//      _velocity += Physics.gravity * Time.fixedDeltaTime;
//      _position += _velocity;
//      _rb.MovePosition(_position);
//  }

    void OnPinchStart(GameObject anchor)
    {
        _pinchAnchor = anchor;
        _elastic = Instantiate(_elasticPrefab, transform.position, Quaternion.identity);
    }

    void OnPinchEnd()
    {
        _rb.velocity = getShotVelocity();
        Destroy(_elastic);
        _elastic = null;
        _pinchAnchor = null;
    }

    Vector3 getShotVelocity()
    {
        var ds = _pinchAnchor.transform.position - transform.position;
        var impulse = -_impulseMultiplier * ds;
        if (impulse.y < 0) impulse = impulse.WithY(0);
        return impulse;
    }

    void Update()
    {
        if (_elastic != null) {
            var ds = _pinchAnchor.transform.position - transform.position;
            _elastic.transform.position = transform.position;
            _elastic.transform.rotation = Quaternion.LookRotation(ds);
            _elastic.transform.localScale = new Vector3(1, 1, ds.magnitude);

            _predictionLine.SetPrediction(_rb.position, getShotVelocity());
        }
    }
}
