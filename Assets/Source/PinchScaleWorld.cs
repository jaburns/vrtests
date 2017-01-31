using UnityEngine;

public class PinchScaleWorld : MonoBehaviour
{
    [SerializeField] GameObject _ovrLeftHandAnchor;
    [SerializeField] GameObject _ovrRightHandAnchor;
    [SerializeField] float _minScale = 0.05f;

    float _startingY;

    GameObject _hookParent;
    Transform _originalParent;
    float _initialHandRadians;
    float _initialHandDistance;
    float _initialScale;

    void Awake()
    {
        _startingY = transform.position.y;
    }

    void Update()
    {
        var wasHooked = _hookParent != null;
        var hookedNow = OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.RIndexTrigger);

        if (!hookedNow) {
            if (wasHooked) {
                transform.parent = _originalParent;
                Destroy(_hookParent);
            }
            return;
        }

        var leftPos = _ovrLeftHandAnchor.transform.position;
        var rightPos = _ovrRightHandAnchor.transform.position;
        var deltaPos = rightPos - leftPos;
        var handMidpoint = (rightPos + leftPos) / 2;
        var handDistance = deltaPos.magnitude;
        var handRadians = Mathf.Atan2(deltaPos.z, -deltaPos.x);

        if (!wasHooked) {
            _hookParent = new GameObject("PinchScaleWorld_PARENT");
            _hookParent.transform.position = handMidpoint;

            _originalParent = transform.parent;
            transform.parent = _hookParent.transform;

            _initialHandRadians = handRadians;
            _initialHandDistance = handDistance;
        }

        _hookParent.transform.rotation = Quaternion.Euler(0, (handRadians - _initialHandRadians) * Mathf.Rad2Deg, 0);

        var scale = handDistance / _initialHandDistance;
        var absScale = scale * transform.localScale.x;
        if (absScale < _minScale) absScale = _minScale;
        if (absScale > 1f) absScale = 1f;
        scale = absScale / transform.localScale.x;
        _hookParent.transform.localScale = new Vector3(scale, scale, scale);

        _hookParent.transform.position = handMidpoint;

        var newAbsoluteY = Mathf.Lerp(handMidpoint.y, _startingY, (absScale - _minScale) / (1f - _minScale));
        transform.position = new Vector3(transform.position.x, newAbsoluteY, transform.position.z);
    }
}