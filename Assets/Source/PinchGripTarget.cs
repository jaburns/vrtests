using UnityEngine;

public class PinchGripTarget : MonoBehaviour
{
    struct RBProps
    {
        public float mass;
        public float drag;
        public float angularDrag;
        public bool useGravity;
        public RigidbodyInterpolation interpolation;
        public CollisionDetectionMode collisionDetectionMode;
        public RigidbodyConstraints constraints;
    }

    RBProps _rbProps;
    Vector3 _lastMeasuredPos;
    Vector3 _velocityEstimate;

    public void StartGrip(Transform gripAnchor)
    {
        var rb = GetComponent<Rigidbody>();
        _rbProps = new RBProps {
            mass = rb.mass,
            drag = rb.drag,
            angularDrag = rb.angularDrag,
            useGravity = rb.useGravity,
            interpolation = rb.interpolation,
            collisionDetectionMode = rb.collisionDetectionMode,
            constraints = rb.constraints
        };

        Destroy(rb);
        transform.parent = gripAnchor;
    }

    void FixedUpdate()
    {
        _velocityEstimate = (transform.position - _lastMeasuredPos) / Time.fixedDeltaTime;
        _lastMeasuredPos = transform.position;
    }

    public void EndGrip()
    {
        transform.parent = null;

        var rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = _rbProps.mass;
        rb.drag = _rbProps.drag;
        rb.angularDrag = _rbProps.angularDrag;
        rb.useGravity = _rbProps.useGravity;
        rb.interpolation = _rbProps.interpolation;
        rb.collisionDetectionMode = _rbProps.collisionDetectionMode;
        rb.constraints = _rbProps.constraints;

        rb.velocity = _velocityEstimate;
    }
}
