using UnityEngine;

public class SwimController : MonoBehaviour
{
    [SerializeField] GameObject _rightHand;
    [SerializeField] GameObject _leftHand;
    [SerializeField] TextMesh _text;
    [SerializeField] bool _allowSuperSpeed;
    [SerializeField] bool _disallowY;

    [SerializeField] float _minimumSpeed = 0.01f;
    [SerializeField] float _stopGripTimeDiff = 1f;

    GameObject _hookedHand;
    OVRInput.RawButton _hookButton;
    Vector3 _lastHookPos;
    Vector3 _velocity;
    Vector3 _stagedVel;
    

    float _leftPressTime;
    float _rightPressTime;

    void Awake()
    {
        _hookedHand = null;
    }

    void Update()
    {
        checkInputs();
        updatePosition();
    }

    void updatePosition()
    {
        if (_hookedHand != null) {
            if (OVRInput.GetUp(_hookButton)) {
                _hookedHand = null;
                _velocity += _stagedVel;
                if (_velocity.sqrMagnitude < _minimumSpeed * _minimumSpeed) _velocity = Vector3.zero;
                if (_text != null) {
                    _text.text = _velocity.magnitude.ToString();
                }
                _stagedVel = Vector3.zero;
            }
            else {
                var newHookPos = _hookedHand.transform.position - transform.position;
                var handVel = newHookPos - _lastHookPos;
                _lastHookPos = newHookPos;
                _stagedVel = -handVel;
            }
        }

        var totalVel = _velocity + _stagedVel;
        if (_disallowY) totalVel.y = 0;

        transform.position += totalVel;
    }

    void checkInputs()
    {
        var checkRight = _hookedHand == null || _hookedHand == _leftHand;
        var checkLeft = _hookedHand == null || _hookedHand == _rightHand;

        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger)) {
            _rightPressTime = Time.timeSinceLevelLoad;
            if (checkRight) {
                if (!_allowSuperSpeed) _velocity = Vector3.zero;
                _hookedHand = _rightHand;
                _hookButton = OVRInput.RawButton.RHandTrigger;
                _lastHookPos = _hookedHand.transform.position - transform.position;
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger)) {
            _leftPressTime = Time.timeSinceLevelLoad;
            if (checkLeft) {
                if (!_allowSuperSpeed) _velocity = Vector3.zero;
                _hookedHand = _leftHand;
                _hookButton = OVRInput.RawButton.LHandTrigger;
                _lastHookPos = _hookedHand.transform.position - transform.position;
            }
        }

        if (_allowSuperSpeed && OVRInput.Get(OVRInput.RawButton.LHandTrigger) && OVRInput.Get(OVRInput.RawButton.RHandTrigger)) {
            var deltaPressTime = Mathf.Abs(_leftPressTime - _rightPressTime);
            if (deltaPressTime < _stopGripTimeDiff) {
                _velocity = Vector3.zero;
            }
        }
    }
}
