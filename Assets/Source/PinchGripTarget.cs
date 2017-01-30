using UnityEngine;

public class PinchGripTarget : MonoBehaviour
{
    Rigidbody _rb;
    Transform _target;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void StartGrip(Transform gripAnchor)
    {
        _target = gripAnchor;
        _rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (_target != null) {
            _rb.MovePosition(_target.position);
        }
    }

    public void EndGrip()
    {
        _target = null;
        _rb.isKinematic = false;
    }
}
