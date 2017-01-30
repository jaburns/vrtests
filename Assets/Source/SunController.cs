using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField] GameObject _trackTarget;

    void Awake()
    {
        transform.parent = _trackTarget.transform;
        transform.localPosition = Vector3.zero;
    }
}