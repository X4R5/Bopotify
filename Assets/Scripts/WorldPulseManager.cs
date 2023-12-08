using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WorldPulseManager : MonoBehaviour
{
    float _bpm;
    AudioSource _audioSource;
    [SerializeField] Intervals[] _intervals;

    private void Start()
    {
        _bpm = BeatManager.Instance.GetBPM();
        _audioSource = BeatManager.Instance.GetAudioSource();
    }

    private void Update()
    {
        foreach (Intervals interval in _intervals)
        {
            float sampledTime = _audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm));
            interval.CheckForNewInterval(sampledTime);
        }
    }

}


[System.Serializable]
public class Intervals
{
    [SerializeField] float _steps;
    [SerializeField] UnityEvent _trigger;
    float _lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60 / bpm * _steps;
    }

    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}