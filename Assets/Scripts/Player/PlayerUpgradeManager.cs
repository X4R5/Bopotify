using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    public static PlayerUpgradeManager Instance;

    List<UpgradeScriptableObject> _addedUpgrades = new List<UpgradeScriptableObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        UpgradeSelectionManager.Instance.ShowUpgrades();
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
