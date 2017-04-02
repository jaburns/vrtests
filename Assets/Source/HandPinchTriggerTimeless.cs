using UnityEngine;
using jaburns.UnityTools;

public class HandPinchTriggerTimeless : MonoBehaviour
{
    const OVRInput.RawButton  LEFT_BUTTON = OVRInput.RawButton.LIndexTrigger;
    const OVRInput.RawButton RIGHT_BUTTON = OVRInput.RawButton.RIndexTrigger;

    [SerializeField] float _radius;

    GameObject _leftAnchor;
    GameObject _rightAnchor;

    bool _leftHovering;
    bool _leftPinching;
    bool _rightHovering;
    bool _rightPinching;

    public GameEvent<GameObject> PinchStart { get; private set; }
    public GameEvent PinchEnd { get; private set; }

    void Awake()
    {
        PinchStart = new GameEvent<GameObject>();
        PinchEnd = new GameEvent();
    }

    void Start()
    {
        _rightAnchor = GameObject.Find(HandPinchTriggers.RIGHT_PINCH_OBJ_NAME);
        _leftAnchor = GameObject.Find(HandPinchTriggers.LEFT_PINCH_OBJ_NAME);
    }

    void Update()
    {
        _leftHovering = (_leftAnchor.transform.position - transform.position).sqrMagnitude < _radius*_radius;
        _rightHovering = (_rightAnchor.transform.position - transform.position).sqrMagnitude < _radius*_radius;

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
