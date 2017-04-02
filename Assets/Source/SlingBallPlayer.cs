using UnityEngine;

public class SlingBallPlayer : MonoBehaviour
{
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A)) {
            transform.position = FindObjectOfType<SlingBall>().transform.position - Vector3.up;
        }

        Time.timeScale = 1f - OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger);
    }
}
