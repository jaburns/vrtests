using UnityEngine;
using UnityEngine.SceneManagement;

public class SwordTargetKill : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.name == "Line" || c.name == "Sphere") return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
