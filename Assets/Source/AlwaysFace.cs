using UnityEngine;

public class AlwaysFace : MonoBehaviour
{
    [SerializeField] GameObject _target;

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(_target.transform.position - transform.position);
    }
}