using UnityEngine;

public class GravitationalBody : MonoBehaviour
{
    [SerializeField] float _mass = 1;

    public Rigidbody rb { get; private set; }
    public float mass { get { return _mass; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void IncreaseMass(float newMass)
    {
        var initialRadius = Mathf.Pow(_mass / Mathf.PI * 3f / 4f, 1f / 3f);
        var newRadius = Mathf.Pow((_mass + newMass) / Mathf.PI * 3f / 4f, 1f / 3f);

        _mass += newMass;

        var newScale = newRadius / initialRadius * transform.localScale.x;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
