using UnityEngine;

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
        if (targ != null && targ.enabled) {
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
