using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    public static PlayerUpgradeManager Instance;

    [SerializeField] float _neededXpForNextLevel;
    float _currentXp;
    int _currentLevel;

    List<UpgradeScriptableObject> _addedUpgrades = new List<UpgradeScriptableObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _currentLevel = 1;
        _currentXp = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }
    }

    public void AddXp(float xp)
    {
        _currentXp += xp;
        if (_currentXp >= _neededXpForNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _currentLevel++;
        _neededXpForNextLevel += 50;
        _currentXp = 0;
        UpgradeSelectionManager.Instance.ShowUpgrades();
        Debug.Log("Level Up! " + _currentLevel);
    }

    public void AddUpgrade(UpgradeScriptableObject upgrade)
    {
        if(upgrade._dashUpgrade._explosionPrefabOnEnd != null)
        {
            foreach (var addedupgrade in _addedUpgrades)
            {
                addedupgrade._dashUpgrade._explosionPrefabOnEnd = null;
            }
        }

        if (upgrade._dashUpgrade._explosionPrefabOnStart != null)
        {
            foreach (var addedupgrade in _addedUpgrades)
            {
                addedupgrade._dashUpgrade._explosionPrefabOnStart = null;
            }
        }

        var newUpgrade = Instantiate(upgrade);

        _addedUpgrades.Add(newUpgrade);

        Debug.Log(newUpgrade._upgradeDescription);
    }

    public List<UpgradeScriptableObject> GetUpgrades()
    {
        return _addedUpgrades;
    }
}
