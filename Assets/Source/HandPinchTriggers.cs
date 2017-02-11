using UnityEngine;

public class HandPinchTriggers : MonoBehaviour
{
    public const string LEFT_PINCH_OBJ_NAME  = "HandPinchTriggers_LEFT";
    public const string RIGHT_PINCH_OBJ_NAME = "HandPinchTriggers_RIGHT";

    [SerializeField] GameObject _ovrLeftHandAnchor;
    [SerializeField] GameObject _ovrRightHandAnchor;
    [SerializeField] float _radius = 0.05f;

    void Awake()
    {
        buildPinchPointObject(_ovrLeftHandAnchor .transform, _radius, false);
        buildPinchPointObject(_ovrRightHandAnchor.transform, _radius, true);
    }

    static void buildPinchPointObject(Transform parent, float radius, bool isRight)
    {
        var xOffsetMultiplier = isRight ? -1f : 1f;

        var pinchObj = new GameObject(isRight ? RIGHT_PINCH_OBJ_NAME : LEFT_PINCH_OBJ_NAME);
        pinchObj.transform.parent = parent;
        pinchObj.transform.localPosition = new Vector3(0.02f * xOffsetMultiplier, -0.025f, 0.01f);

        var rb = pinchObj.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        pinchObj.layer = LayerMask.NameToLayer("PinchTrigger");

        var col = pinchObj.AddComponent<SphereCollider>();
        col.radius = radius;
    }
}