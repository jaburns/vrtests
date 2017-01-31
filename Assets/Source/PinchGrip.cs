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