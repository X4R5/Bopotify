using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance { get; private set; }

    [SerializeField] Transform _holder;
    [SerializeField] GameObject _heartPrefab;

    Stack<GameObject> _heartObjects = new Stack<GameObject>();

    [SerializeField] List<AnimationClip> _danceAnims = new List<AnimationClip>();

    int _health;

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

        var h = PlayerPrefs.GetInt("Health");

        if(h == 0)
        {
            PlayerPrefs.SetInt("Health", 6);
            _health = 6;
        }
        else
        {
            _health = h;
        }

        for (int i = 0; i < _health; i++)
        {
            var newHeart = Instantiate(_heartPrefab, _holder);
            _heartObjects.Push(newHeart);
        }
    }

    public void TakeDamage()
    {
        _health--;

        if(_health <= 0)
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
        var allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in allEnemies)
        {
            _danceAnims.Shuffle();
            //enemy.GetComponent<Animator>().Play(_danceAnims[0],0,0);
        }
    }
}
