using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    [SerializeField] AudioSource _audioSource;
    [SerializeField] float _bpm;
    float _secondsPerBeat;
    float _nextBeatTime;
    float _lastBeatTime;

    [SerializeField] GameObject _rhythmCanvas;

    [SerializeField] TMP_Text _perfectText, _goodText, _missText;

    public Image[] _nextBeatProgressImages;

    bool _canAction = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    private void Start()
    {
        _secondsPerBeat = 60f / _bpm;
        _lastBeatTime = Time.time;
        _nextBeatTime = Time.time + _secondsPerBeat;
        HideText();
    }

    private void Update()
    {
        float _currentTime = Time.time;

        if (_currentTime >= _nextBeatTime)
        {
            _lastBeatTime = _nextBeatTime;
            _nextBeatTime += _secondsPerBeat;
            foreach (Image image in _nextBeatProgressImages)
            {
                image.fillAmount = 1;
                image.fillAmount = 0;
            }
        }

        foreach (Image image in _nextBeatProgressImages)
        {
            image.fillAmount = (_currentTime - (_nextBeatTime - _secondsPerBeat)) / _secondsPerBeat;
        }
    }

    public float GetBPM()
    {
        return _bpm;
    }

    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }

    public BeatResult GetBeatResult()
    {
        _canAction = false;

        float currentTime = Time.time;
        float timeSinceLastBeat = currentTime - _lastBeatTime;
        float timeUntilNextBeat = _nextBeatTime - currentTime;

        float perfectWindow = 0.08f;
        float goodWindow = 0.17f;

        HideText();
        
        if ((Mathf.Abs(timeSinceLastBeat) < perfectWindow) || (Mathf.Abs(timeUntilNextBeat) < perfectWindow))
        {
            _perfectText.gameObject.SetActive(true);
            Invoke("HideText", 0.3f);
            Invoke("EnableAction", 0.1f);
            return BeatResult.Perfect;
        }
        else if ((Mathf.Abs(timeSinceLastBeat) < goodWindow) || (Mathf.Abs(timeUntilNextBeat) < goodWindow))
        {
            _goodText.gameObject.SetActive(true);
            Invoke("HideText", 0.3f);
            Invoke("EnableAction", 0.1f);
            return BeatResult.Good;
        }
        else
        {
            _missText.gameObject.SetActive(true);
            Invoke("HideText", 0.3f);
            Invoke("EnableAction", 2 * _secondsPerBeat);
            return BeatResult.Miss;
        }
    }

    public bool CanAction()
    {
        return _canAction;
    }

    void EnableAction()
    {
        _canAction = true;
    }

    public void HideText()
    {
        _perfectText.gameObject.SetActive(false);
        _goodText.gameObject.SetActive(false);
        _missText.gameObject.SetActive(false);
    }

    public void ShowCanvas()
    {
        _rhythmCanvas.SetActive(true);
    }

    public void HideCanvas()
    {
        _rhythmCanvas.SetActive(false);
    }

    public void PauseMusic()
    {
        _audioSource.Pause();
    }
    public void PlayMusic()
    {
        _audioSource.Play();
    }

    public float GetNextBeatTime()
    {
        return _nextBeatTime;
    }

    public float GetSecondsPerBeat()
    {
        return _secondsPerBeat;
    }

}
