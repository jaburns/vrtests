using UnityEngine;

public class PinchGrip : MonoBehaviour
{
    [SerializeField] GameObject _ovrLeftHandAnchor;
    [SerializeField] GameObject _ovrRightHandAnchor;

    SphereCollider[] _pinchColliders;

    void Awake()
    {
        _pinchColliders = new SphereCollider[] {
            buildGrip(_ovrLeftHandAnchor .transform,  1f),
            buildGrip(_ovrRightHandAnchor.transform, -1f),
        };
    }

    SphereCollider buildGrip(Transform parent, float xOffsetMultiplier)
    {
        var pinchObj = new GameObject();
        pinchObj.transform.parent = parent;
        pinchObj.transform.localPosition = new Vector3(0.02f * xOffsetMultiplier, -0.025f, 0.01f);

        pinchObj.AddComponent<PinchGripPinchPoint>();

        var sphereCol = pinchObj.AddComponent<SphereCollider>();
        sphereCol.isTrigger = true;
        sphereCol.radius = 0.005f;

        return sphereCol;
    }

}

public class PinchGripPinchPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        var targ = c.GetComponent<PinchGripTarget>();
        if (targ == null) return;
        targ.StartGrip(transform);
    }
}
