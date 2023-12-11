using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance { get; private set; }

    [SerializeField] Transform _holder;
    [SerializeField] GameObject _heartPrefab;

    Stack<GameObject> _heartObjects = new Stack<GameObject>();

    int _currentHealth;
    int _maxHealth;

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

        _currentHealth = PlayerPrefs.GetInt("CurrentHealth");
        _maxHealth = PlayerPrefs.GetInt("MaxHealth");

        for (int i = 0; i < _maxHealth; i++)
        {
            var heart = Instantiate(_heartPrefab, _holder);
            if(i + 1 > _currentHealth)
            {
                heart.transform.GetChild(0).gameObject.SetActive(true);
                heart.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                _heartObjects.Push(heart);
            }
        }
    }

    public void TakeDamage()
    {
        _currentHealth--;
        PlayerPrefs.SetInt("CurrentHealth", _currentHealth);

        if(_currentHealth <= 0)
        {
            Debug.Log("Game Over");
            GameTracker.Instance.GameOver();
            GameOver();
            return;
        }

        var heart = _heartObjects.Pop();
        heart.transform.GetChild(0).gameObject.SetActive(true);
        heart.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void GameOver()
    {
        SceneManager.LoadScene("StartRunMenu");
        Destroy(GameTracker.Instance.gameObject);
        Destroy(BeatManager.Instance.gameObject);
    }

    public void HideCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ShowCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
