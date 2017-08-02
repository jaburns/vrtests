using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    [SerializeField] int _ticksPerEighth = 10;
    [SerializeField] float _audioLatency;
    [SerializeField] AudioSource _hatSound;
    [SerializeField] AudioSource _openHatSound;
    [SerializeField] AudioSource _snareSound;
    [SerializeField] Basketball _basketball;

    int _ticks;

	void FixedUpdate () 
    {
        _ticks++;

        if (_ticks % (8 * _ticksPerEighth) == 0) {
            note();
        }
        else if (_ticks % (2 * _ticksPerEighth) == 0) {
            quarter();
        }
        else if (_ticks % _ticksPerEighth == 0) {
            eighth();
        }
	}

    void eighth()
    {
        switch (_basketball.state) {
            case Basketball.State.Bouncing:
                break;
            case Basketball.State.Grabbed:
                _hatSound.Play();
                break;
            case Basketball.State.Tossed:
                break;
        }
    }

    void quarter()
    {
        switch (_basketball.state) {
            case Basketball.State.Bouncing:
                _hatSound.Play();
                break;
            case Basketball.State.Grabbed:
                _hatSound.Play();
                break;
            case Basketball.State.Tossed:
                _hatSound.Play();
                break;
        }
    }

    void note()
    {
        switch (_basketball.state) {
            case Basketball.State.Bouncing:
                _hatSound.Play();
                _snareSound.Play();
                break;
            case Basketball.State.Grabbed:
                _hatSound.Play();
                break;
            case Basketball.State.Tossed:
                _openHatSound.Play();
                break;
        }
    }

    void Update()
    {
        var subTick = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        var timeInBounce = ((float)_ticks + subTick) / _ticksPerEighth / 8.0f;

        _basketball.UpdateForTime(timeInBounce + 0.01f);
    }
}
