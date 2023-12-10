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
            _isGameOver = true;
            Debug.Log("Game Over");
        }
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}
