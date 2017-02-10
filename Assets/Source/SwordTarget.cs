using UnityEngine;

public class SwordTarget : MonoBehaviour
{
    [SerializeField] float _speed = .5f;

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = -Vector3.forward * _speed;
    }

    public void BindToNext(GameObject next)
    {
        var d = next.transform.position - transform.position;
        var lr = GetComponentInChildren<LineRenderer>();
        lr.transform.localScale = new Vector3(1, 1, d.magnitude);
        lr.transform.rotation = Quaternion.LookRotation(d);
    }
}
