using UnityEngine;

public class PinchGrip : MonoBehaviour
{
    [SerializeField] GameObject _ovrLeftHandAnchor;
    [SerializeField] GameObject _ovrRightHandAnchor;

    SphereCollider[] _pinchColliders;

    void Awake()
    {
        _pinchColliders = new SphereCollider[] {
            buildGrip(_ovrLeftHandAnchor .transform,  1f, OVRInput.RawButton.LIndexTrigger),
            buildGrip(_ovrRightHandAnchor.transform, -1f, OVRInput.RawButton.RIndexTrigger)
        };
    }

    SphereCollider buildGrip(Transform parent, float xOffsetMultiplier, OVRInput.RawButton btn)
    {
        var pinchObj = new GameObject();
        pinchObj.transform.parent = parent;
        pinchObj.transform.localPosition = new Vector3(0.02f * xOffsetMultiplier, -0.025f, 0.01f);

        var pinchPt = pinchObj.AddComponent<PinchGripPinchPoint>();
        pinchPt.SetButton(btn);

        var sphereCol = pinchObj.AddComponent<SphereCollider>();
        sphereCol.isTrigger = true;
        sphereCol.radius = 0.005f;

        return sphereCol;
    }
}

public class PinchGripPinchPoint : MonoBehaviour
{
    OVRInput.RawButton _button;
    PinchGripTarget _potentialTarget;
    PinchGripTarget _curTarget;

    public void SetButton(OVRInput.RawButton btn)
    {
        _button = btn;
    }

    void OnTriggerEnter(Collider c)
    {
        if (_curTarget != null) return;

        var targ = c.GetComponent<PinchGripTarget>();
        if (targ != null) {
            _potentialTarget = targ;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (_curTarget != null || _potentialTarget == null) return;

        var targ = c.GetComponent<PinchGripTarget>();
        if (targ == _potentialTarget) {
            _potentialTarget = null;
        }
    }

    void Update()
    {
        if (_curTarget == null && _potentialTarget != null && OVRInput.GetDown(_button)) {
            _curTarget = _potentialTarget;
            _potentialTarget = null;
            _curTarget.StartGrip(transform);
        }
        else if (_curTarget != null && OVRInput.GetUp(_button)) {
            _curTarget.EndGrip();
            _curTarget = null;
        }
    }
}
