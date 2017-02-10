using UnityEngine;

public class SwordTargetLine : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.name == "Line" || c.name == "Sphere") return;
        Destroy(gameObject);
    }
}
