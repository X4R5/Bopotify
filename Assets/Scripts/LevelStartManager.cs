using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelStartManager : MonoBehaviour
{
    public static LevelStartManager Instance;
    bool _isGameStarted = false;
    GameObject _player;


    private void Awake()
    {
        Instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(LevelStart());
    }

    IEnumerator LevelStart()
    {
        _player.GetComponent<Animator>().SetTrigger("Start");

        yield return new WaitForSeconds(0.1f);

        var randSong = RandomSongSelector.Instance.GetRandomSongSO();
        BeatManager.Instance.SetValues(randSong._bpm, randSong._audioClip);

        yield return new WaitForSeconds(2f);

        BeatManager.Instance.ShowCanvas();

        yield return new WaitForSeconds(3f);

        _isGameStarted = true;

        yield return null;
    }

    public bool IsGameStarted()
    {
        return _isGameStarted;
    }
}
