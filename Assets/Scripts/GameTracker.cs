using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance { get; private set; }

    [SerializeField] int _easyRoomsStartIndex, _easyRoomsEndIndex;
    [SerializeField] int _hardRoomsStartIndex, _hardRoomsEndIndex;
    [SerializeField] int _easyRoomCountToPlay;
    [SerializeField] int _hardRoomCountToPlay;

    [SerializeField] int _easyBossIndex;
    [SerializeField] int _hardBossIndex;

    [SerializeField] bool _isGameOver;


    List<int> _easyRoomIndexes = new List<int>();
    List<int> _hardRoomIndexes = new List<int>();
    int _playedEasyRooms = 0;
    int _playedHardRooms = 0;
    bool _easyBossPlayed = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        for (int i = _easyRoomsStartIndex; i <= _easyRoomsEndIndex; i++)
        {
            _easyRoomIndexes.Add(i);
        }

        for (int i = _hardRoomsStartIndex; i <= _hardRoomsEndIndex; i++)
        {
            _hardRoomIndexes.Add(i);
        }

        _easyRoomIndexes.Shuffle();
        _hardRoomIndexes.Shuffle();
    }

    public void LoadRandomScene()
    {
        int sceneIndex = GetRandomSceneIndex();
        SceneManager.LoadScene(sceneIndex);
    }

    private int GetRandomSceneIndex()
    {
        if(_playedEasyRooms < 3)
        {
            var index = _easyRoomIndexes[0];
            _easyRoomIndexes.RemoveAt(0);
            return index;
        }
        else if(_playedEasyRooms == 3 && !_easyBossPlayed)
        {
            return _easyBossIndex;
        }
        else if(_playedHardRooms < 3)
        {
            var index = _hardRoomIndexes[0];
            _hardRoomIndexes.RemoveAt(0);
            return index;
        }
        else
        {
            return _hardBossIndex;
        }
    }

    public void RoomCompleted()
    {
        if(_playedEasyRooms < 3)
        {
            _playedEasyRooms++;
        }
        else
        {
            _playedHardRooms++;
        }
    }

    public void EasyBossCompleted()
    {
        _easyBossPlayed = true;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
