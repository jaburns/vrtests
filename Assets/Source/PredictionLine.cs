using UnityEngine;

public class PredictionLine : MonoBehaviour
{
    const int STEPS = 30;

    LineRenderer _line;

    void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    public void SetPrediction(Vector3 position, Vector3 velocity)
    {
        _line.positionCount = STEPS;
        var posList = new Vector3[STEPS];
        var curPos = position;
        
        for (int i = 0; i < STEPS; ++i) {
            posList[i] = curPos;
            curPos += velocity * Time.fixedDeltaTime;
            velocity += Physics.gravity * Time.fixedDeltaTime;
        }

        _line.SetPositions(posList);
    }
}
