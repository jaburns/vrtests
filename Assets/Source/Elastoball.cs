using UnityEngine;

public class Elastoball : MonoBehaviour,
    Message<HandPinchTriggers.StartPinchMessage>,
    Message<HandPinchTriggers.EndPinchMessage>
{
    [SerializeField] GameObject _elasticPrefab;
    [SerializeField] float _impulseMultiplier = 1f;

    Rigidbody _rb;
    GameObject _elastic;
    GameObject _pinchAnchor;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.velocity = -5f * Vector3.forward;
    }

    void Update()
    {
        if (_elastic == null) return;

        var ds = _pinchAnchor.transform.position - transform.position;

        _elastic.transform.rotation = Quaternion.LookRotation(ds);
        _elastic.transform.localScale = new Vector3(1, 1, ds.magnitude);
    }

    void Message<HandPinchTriggers.StartPinchMessage>.Receive(HandPinchTriggers.StartPinchMessage msg)
    {
        if (_elastic != null) return;

        _pinchAnchor = msg.anchor;
        _elastic = Instantiate(_elasticPrefab, transform.position, Quaternion.identity);
    }

    void Message<HandPinchTriggers.EndPinchMessage>.Receive(HandPinchTriggers.EndPinchMessage msg)
    {
        if (_elastic == null) return;

        var ds = _pinchAnchor.transform.position - transform.position;
        _rb.AddForce(-_impulseMultiplier * ds, ForceMode.VelocityChange);

        Destroy(_elastic);
        _elastic = null;
        _pinchAnchor = null;
    }
}