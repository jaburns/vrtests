using UnityEngine;

public class PlayerSwordHolder : MonoBehaviour
{
    [SerializeField] GameObject _ovrLeftHandAnchor;
    [SerializeField] GameObject _ovrRightHandAnchor;
    [SerializeField] GameObject _swordCylinder;

    void Awake()
    {
    }

    void Update()
    {
        _swordCylinder.transform.position = (_ovrLeftHandAnchor.transform.position + _ovrRightHandAnchor.transform.position) / 2;
        _swordCylinder.transform.rotation = Quaternion.LookRotation(_ovrLeftHandAnchor.transform.position - _ovrRightHandAnchor.transform.position);
        _swordCylinder.transform.localScale = new Vector3(
            _swordCylinder.transform.localScale.x,
            _swordCylinder.transform.localScale.y,
            0.5f * (_ovrLeftHandAnchor.transform.position - _ovrRightHandAnchor.transform.position).magnitude
        );
    }
}
