using UnityEngine;
using jaburns.UnityTools;

public class MountainRider : MonoBehaviour
{
    [SerializeField] GameObject _ovrLeftHandAnchor;
    [SerializeField] GameObject _ovrRightHandAnchor;

    void Update()
    {
        transform.position = transform.position.WithXZ(_ovrRightHandAnchor.transform.position);
    }
}
