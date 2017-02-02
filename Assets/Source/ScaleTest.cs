using UnityEngine;

public class ScaleTest : MonoBehaviour
{
    public float Amount = 1.01f;

    void Awake()
    {
    }

    void Update()
    {
        transform.localScale = transform.localScale * Amount;
    }
}
