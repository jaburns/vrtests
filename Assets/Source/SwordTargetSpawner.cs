using UnityEngine;

public class SwordTargetSpawner : MonoBehaviour
{
    [SerializeField] GameObject _swordTargetPrefab;
    [SerializeField] float _period;

    GameObject _lastGo;

    float _counter;

    void Awake()
    {
        _counter = 0;
    }

    void Update()
    {
        _counter -= Time.deltaTime;
        if (_counter < 0) {
            _counter = _period;
            var go = Instantiate(_swordTargetPrefab, transform.position + 1.5f*(2f * Random.value - 1f) * Vector3.right + 2f*(Random.value - .5f) * Vector3.up, Quaternion.identity);
            if (_lastGo != null) {
                makeLine(_lastGo, go);
            }
            _lastGo = go;
        }
    }

    static void makeLine(GameObject a, GameObject b)
    {
        a.GetComponent<SwordTarget>().BindToNext(b);
    }
}
