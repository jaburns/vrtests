using UnityEngine;
using System.Collections.Generic;

public class HandPinchTriggers : MonoBehaviour
{
    abstract public class PinchMessage {
        public GameObject anchor;
    }

    public class StartPinchMessage : PinchMessage {}
    public class EndPinchMessage : PinchMessage {}

    [SerializeField] GameObject _ovrLeftHandAnchor;
    [SerializeField] GameObject _ovrRightHandAnchor;
    [SerializeField] float _radius = 0.05f;

    Collider _rightPinch;
    Collider _leftPinch;

    void Awake()
    {
        _leftPinch  = buildPinchPointObject(_ovrLeftHandAnchor .transform, _radius, false);
        _rightPinch = buildPinchPointObject(_ovrRightHandAnchor.transform, _radius, true);
    }

    static Collider buildPinchPointObject(Transform parent, float radius, bool isRight)
    {
        var xOffsetMultiplier = isRight ? -1f : 1f;

        var pinchObj = new GameObject("HandPinchTriggers_Collider");
        pinchObj.transform.parent = parent;
        pinchObj.transform.localPosition = new Vector3(0.02f * xOffsetMultiplier, -0.025f, 0.01f);

        var col = pinchObj.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = radius;

        pinchObj.AddComponent<HandPinchTriggerCollider>().SetHand(isRight);

        return col;
    }
}

[AddComponentMenu("")]
public class HandPinchTriggerCollider : MonoBehaviour
{
    List<GameObject> _targets;
    List<GameObject> _activeTargets;

    OVRInput.RawButton _button;
    bool _isRightHand;

    public void SetHand(bool isRight)
    {
        _isRightHand = isRight;
        _button = isRight ? OVRInput.RawButton.RIndexTrigger : OVRInput.RawButton.LIndexTrigger;
    }

    void Awake()
    {
        _targets = new List<GameObject>();
        _activeTargets = new List<GameObject>();
    }

    void OnTriggerEnter(Collider c)
    {
        if (!_targets.Contains(c.gameObject)) {
            _targets.Add(c.gameObject);
        }
    }

    void Update()
    {
        if (OVRInput.GetDown(_button)) {
            Debug.Log("X");
            foreach (var t in _targets) {
                t.SendTypedMessage(new HandPinchTriggers.StartPinchMessage {
                    anchor = gameObject
                });
                _activeTargets.Add(t);
            }
        }
        else if (OVRInput.GetUp(_button)) {
            foreach (var t in _activeTargets) {
                t.SendTypedMessage(new HandPinchTriggers.EndPinchMessage {
                    anchor = gameObject
                });
            }
            _activeTargets.Clear();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (_targets.Contains(c.gameObject)) {
            _targets.Remove(c.gameObject);
        }
    }
}