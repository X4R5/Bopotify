using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] GameObject _canvas;
    [SerializeField] Slider _masterVolumeSlider, _musicVolumeSlider;

    private void Awake()
    {
        if(PlayerPrefs.GetFloat("MasterVolume") == 0)
        {
            PlayerPrefs.SetFloat("MasterVolume", 0.5f);
        }

        if (PlayerPrefs.GetFloat("MusicVolume") == 0)
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
    }
    private void Start()
    {
        _masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");

        _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        MasterVolumeSlider();
        MusicVolumeSlider();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_canvas.activeInHierarchy)
        {
            _canvas.SetActive(true);
            BeatManager.Instance.HideCanvas();
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && _canvas.activeInHierarchy)
        {
            _canvas.SetActive(false);
            BeatManager.Instance.ShowCanvas();
            Time.timeScale = 1;
        }
    }

    public void ContinueButton()
    {
        _canvas.SetActive(false);
        BeatManager.Instance.ShowCanvas();
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("StartRunMenu");
    }

    public void MasterVolumeSlider()
    {
        AudioListener.volume = _masterVolumeSlider.value;
    }

    public void MusicVolumeSlider()
    {
        var audioSource = BeatManager.Instance.GetAudioSource();

        audioSource.volume = _musicVolumeSlider.value;
    }
}
