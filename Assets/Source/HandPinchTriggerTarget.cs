using UnityEngine;
using jaburns.UnityTools;

public class HandPinchTriggerTarget : MonoBehaviour
{
    const OVRInput.RawButton  LEFT_BUTTON = OVRInput.RawButton.LIndexTrigger;
    const OVRInput.RawButton RIGHT_BUTTON = OVRInput.RawButton.RIndexTrigger;

    public GameEvent<GameObject> PinchStart { get; private set; }
    public GameEvent PinchEnd { get; private set; }

    bool _leftHovering;
    bool _leftPinching;
    bool _rightHovering;
    bool _rightPinching;

    GameObject _leftAnchor;
    GameObject _rightAnchor;

    void Awake()
    {
        PinchStart = new GameEvent<GameObject>();
        PinchEnd = new GameEvent();
    }

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("ENTER");
        if (c.gameObject.name == HandPinchTriggers.LEFT_PINCH_OBJ_NAME) {
            _leftAnchor = c.gameObject;
            _leftHovering = true;
        }
        else if (c.gameObject.name == HandPinchTriggers.RIGHT_PINCH_OBJ_NAME) {
            _rightAnchor = c.gameObject;
            _rightHovering = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.name == HandPinchTriggers.LEFT_PINCH_OBJ_NAME) {
            _leftHovering = false;
        }
        else if (c.gameObject.name == HandPinchTriggers.RIGHT_PINCH_OBJ_NAME) {
            _rightHovering = false;
        }
    }

    void Update()
    {
        if (!_leftPinching && !_rightPinching) {
            if (OVRInput.GetDown(LEFT_BUTTON) && _leftHovering) {
                _leftPinching = true;
                PinchStart.Broadcast(_leftAnchor);
            }
            else if (OVRInput.GetDown(RIGHT_BUTTON) && _rightHovering) {
                _rightPinching = true;
                PinchStart.Broadcast(_rightAnchor);
            }
        }
        else if (_leftPinching && OVRInput.GetUp(LEFT_BUTTON)) {
            _leftPinching = false;
            PinchEnd.Broadcast();
        }
        else if (_rightPinching && OVRInput.GetUp(RIGHT_BUTTON)) {
            _rightPinching = false;
            PinchEnd.Broadcast();
        }
    }
}
