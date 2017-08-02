using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basketball : MonoBehaviour 
{
    [SerializeField] float _height;
    [SerializeField] GameObject _hand;
    [SerializeField] Vector3 _gravity;

    float _originalY; 
    Vector3 _lastPos;
    Vector3 _velocity;
    Vector3 _tossPos;
    float _targetT;
    float _startT;

    public enum State {
        Bouncing, Grabbed, Tossed
    }

    public State state { get; private set; }


    void Awake()
    {
        _originalY = transform.position.y;
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Grabbed:
                _velocity = (transform.position - _lastPos) / Time.fixedDeltaTime;
                _lastPos = transform.position;
                break;
        }
    }

    public void UpdateForTime(float t)
    {
        var grabbedNow = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
        var pos = transform.position;

        if (grabbedNow) {
            state = State.Grabbed;
        }

        switch (state) {
            case State.Bouncing:
                pos.y = getBouncerY(t);
                break;
            
            case State.Grabbed:
                pos = _hand.transform.position;
                if (!grabbedNow) {
                    state = State.Tossed;
                    _tossPos = pos;
                    _targetT = -1f;
                }
                break;

            case State.Tossed:
                _velocity += _gravity * Time.deltaTime;
                _tossPos += _velocity * Time.deltaTime * (_targetT > 0 ? 0.01f : 1.0f);

                if (pos.y < 1.0f) {
                    if (_targetT < 0f) {
                        _targetT = Mathf.Ceil(t);
                        _startT = t;
                    }
                    var bpos = _tossPos;
                    bpos.y = getBouncerY(t);
                    pos = Vector3.Lerp(_tossPos, bpos, (t - _startT) / (_targetT - _startT));
                    if (t >= _targetT) {
                        state = State.Bouncing;
                    }
                } else {
                    pos = _tossPos;
                }

                break;
        }

        transform.position = pos;
    }

    float getBouncerY(float t)
    {
        var t2 = bouncer(t % 1f);
        return _originalY + _height * t2;
    }

    static float bouncer(float x)
    {
        var x1 = x - 0.5f;
        return 1f - 4*x1*x1;
    }
}