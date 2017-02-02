using UnityEngine; 
public class PinchScalePlayer : MonoBehaviour
{
    [SerializeField] GameObject _ovrLeftHandAnchor;
    [SerializeField] GameObject _ovrRightHandAnchor;
    [SerializeField] float _godScale = 20f;
    [SerializeField] float _percentDistScaleTrigger = .2f;
    [SerializeField] float _transitionTime = 1f;

    GameObject _rightPinch;
    GameObject _leftPinch;
    GameObject _hookPrimary;
    GameObject _hookSecondary;

    bool _godMode = false;
    bool _changingMode = false;
    float _changeModeT = 0;
    Vector3 _initialPosition;
    Vector3 _targetPosition;
    Vector3? _previousPrimaryHookPos;
    float _initialHandDistance;

    float? _previousHandAngle;

    void Awake()
    {
        _leftPinch  = buildPinchPointObject(_ovrLeftHandAnchor .transform,  1f);
        _rightPinch = buildPinchPointObject(_ovrRightHandAnchor.transform, -1f);
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }

    static GameObject buildPinchPointObject(Transform parent, float xOffsetMultiplier)
    {
        var pinchObj = new GameObject("PinchScalePlayer_PinchPoint");
        pinchObj.transform.parent = parent;
        pinchObj.transform.localPosition = new Vector3(0.02f * xOffsetMultiplier, -0.025f, 0.01f);
        return pinchObj;
    }

    void Update()
    {
        if (_hookPrimary == null) {
            _previousPrimaryHookPos = null;
        }

        if (_changingMode) {
            updateModeChange();
        }
        else {
            updateInMode();
        }
    }

    void updateModeChange()
    {
        _changeModeT += Time.deltaTime / _transitionTime;
        _changingMode = _changeModeT < 1f;
        if (!_changingMode) _changeModeT = 1f;

        var smoothedT = Mathf.SmoothStep(0f, 1f, _changeModeT);

        var scale = Mathf.Pow(_godScale, _godMode ? smoothedT : (1f - smoothedT)); 
        transform.localScale = Vector3.one * scale;

        var posT = (scale - 1f) / (_godScale - 1f);
        if (!_godMode) posT = 1f - posT;

        transform.position = Vector3.Lerp(_initialPosition, _targetPosition, posT);
    }

    void changeGodMode(bool godMode) 
    {
        _godMode = godMode;
        _changeModeT = 0f;
        _changingMode = true;
        _initialPosition = transform.position;

        if (godMode) {
            _targetPosition = new Vector3(transform.position.x, -25, transform.position.z);
            // TODO Place the board at hand height and take in to account the head position of the player.
        } else {
            _targetPosition = new Vector3(_hookPrimary.transform.position.x, 0, _hookPrimary.transform.position.z);
        }
    }

    void updateInMode()
    {
        checkHookButtonStates();

        if (_hookPrimary != null && _godMode) {
            // TODO Figure out how to move and rotate the board in the same frame.
            if (_hookSecondary == null) {
                var newPos = _hookPrimary.transform.position - transform.position;
                if (_previousPrimaryHookPos.HasValue) {
                    var deltaPos = newPos - _previousPrimaryHookPos.Value;
                    transform.position -= deltaPos;
                    _previousHandAngle = null;
                }
                _previousPrimaryHookPos = newPos;
            } else {
                var dHook = _hookSecondary.transform.position - _hookPrimary.transform.position;
                var newAngle = Mathf.Rad2Deg * Mathf.Atan2(dHook.z, -dHook.x) - transform.rotation.eulerAngles.y;
                Debug.Log(newAngle);
                if (_previousHandAngle.HasValue) {
                    var deltaAngle = newAngle - _previousHandAngle.Value;
                    transform.RotateAround(_hookPrimary.transform.position, Vector3.up, -deltaAngle);
                    RenderSettings.skybox.SetFloat("_Rotation", -transform.rotation.eulerAngles.y);
                    _previousPrimaryHookPos = null;
                }
                _previousHandAngle = newAngle;
            }
        }

        if (_hookPrimary != null && _hookSecondary != null) {
            var dp = (_hookPrimary.transform.position - _hookSecondary.transform.position) / transform.localScale.x;
            var dist = dp.magnitude;

            if (_godMode) {
                if (dist > (1f + _percentDistScaleTrigger) * _initialHandDistance) {
                    changeGodMode(false);
                }
            } else {
                if (dist < 1f / (1f + _percentDistScaleTrigger) * _initialHandDistance) {
                    changeGodMode(true);
                }
            }
        }
    }

    void startTwoHookGrip()
    {
        var deltaPos = (_hookPrimary.transform.position - _hookSecondary.transform.position) / transform.localScale.x;
        _initialHandDistance = deltaPos.magnitude;
    }

    void checkHookButtonStates()
    {
        if (! _hookPrimary) {
            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger)) {
                _hookPrimary = _leftPinch;
            }
            else if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) {
                _hookPrimary = _rightPinch;
            }
        }

        if (! _hookSecondary) {
            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && _hookPrimary == _rightPinch) {
                _hookSecondary = _leftPinch;
            }
            else if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && _hookPrimary == _leftPinch) {
                _hookSecondary = _rightPinch;
            }

            if (_hookSecondary) {
                startTwoHookGrip();
            }
        }

        if (_hookSecondary) {
            if (!OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && _hookSecondary == _leftPinch) {
                _hookSecondary = null;
            }
            else if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && _hookSecondary == _rightPinch) {
                _hookSecondary = null;
            }
        }

        if (_hookPrimary) {
            if (!OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && _hookPrimary == _leftPinch) {
                _hookPrimary = _hookSecondary;
                _hookSecondary = null;
                _previousPrimaryHookPos = null;
            }
            else if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && _hookPrimary == _rightPinch) {
                _hookPrimary = _hookSecondary;
                _hookSecondary = null;
                _previousPrimaryHookPos = null;
            }
        }
    }
}