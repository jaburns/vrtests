using UnityEngine;

public class MountainSlider : MonoBehaviour
{
    [SerializeField] float _speed;

    void Update()
    {
        transform.localPosition -= _speed * Vector3.forward;
    }
}
