using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] int _totalEnemyCount = 0;
    int _killedEnemyCount = 0;
    bool _isGameOver = false;

    [SerializeField] bool _isLastLevel = false;
    [SerializeField] GameObject _gameOverPanel;

    private void Awake()
    {
        Instance = this;
    }
    
    public void EnemyKilled()
    {
        _killedEnemyCount++;

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (_killedEnemyCount >= _totalEnemyCount)
        {
            if (_isLastLevel)
            {
                _gameOverPanel.SetActive(true);
                Time.timeScale = 0;
                BeatManager.Instance.HideCanvas();
                return;
            }

            _isGameOver = true;
            PlayerUpgradeManager.Instance.LevelUp();
            GameTracker.Instance.RoomCompleted();
            //GameTracker.Instance.LoadRandomScene();
        }
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}
