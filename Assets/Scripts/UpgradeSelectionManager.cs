using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionManager : MonoBehaviour
{
    public static UpgradeSelectionManager Instance;

    List<UpgradeScriptableObject> _allUpgrades = new List<UpgradeScriptableObject>();
    List<UpgradeScriptableObject> _selectedUpgrades = new List<UpgradeScriptableObject>();
    [SerializeField] GameObject _canvas;

    [SerializeField] TMP_Text[] _optionTexts;
    [SerializeField] Button[] _buttons;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _canvas = transform.GetChild(0).gameObject;
        UpgradeScriptableObject[] upgradesArray = Resources.LoadAll<UpgradeScriptableObject>("Upgrades");
        _allUpgrades.AddRange(upgradesArray);

        int i = 0;
        foreach (var button in _buttons)
        {
            int j = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SelectOption(j));
            i++;
        }
    }

    public void ShowCanvas()
    {
        BeatManager.Instance.HideCanvas();
        _canvas.SetActive(true);

        Time.timeScale = 0f;
    }

    public void HideCanvas()
    {
        BeatManager.Instance.ShowCanvas();
        _canvas.SetActive(false);
        Time.timeScale = 1f;
    }
    

    void RandomUpgrades(int count)
    {
        _selectedUpgrades.Clear();

        if (_allUpgrades.Count < count)
        {
            Debug.LogError("Not enough upgrades available to select.");
            return;
        }

        List<UpgradeScriptableObject> shuffledUpgrades = new List<UpgradeScriptableObject>(_allUpgrades);
        shuffledUpgrades.Shuffle();

        for (int i = 0; i < count; i++)
        {
            _selectedUpgrades.Add(shuffledUpgrades[i]);
        }

        UpdateOptions();
    }

    private void UpdateOptions()
    {
        for (int i = 0; i < 3; i++)
        {
            _optionTexts[i].text = _selectedUpgrades[i]._upgradeDescription;
        }
    }

    void SelectOption(int selection)
    {
        PlayerUpgradeManager.Instance.AddUpgrade(_selectedUpgrades[selection]);
        PlayerAttack.Instance.UpdateAttackValues(_selectedUpgrades[selection]._attackUpgrade._perfectAttackIncreasePercent, _selectedUpgrades[selection]._attackUpgrade._goodAttackIncreasePercent, _selectedUpgrades[selection]._attackUpgrade._perfectAttackRangeIncreasePercent, _selectedUpgrades[selection]._attackUpgrade._goodAttackRangeIncreasePercent);
        PlayerController.Instance.UpdateStats(_selectedUpgrades[selection]._moveSpeedInreasePercent, _selectedUpgrades[selection]._damageDecreasePercent, _selectedUpgrades[selection]._dashUpgrade._perfectDashDistanceIncreasePercent, _selectedUpgrades[selection]._dashUpgrade._goodDashDistanceIncreasePercent);

        HideCanvas();
    }

    public void ShowUpgrades()
    {
        RandomUpgrades(3);

        ShowCanvas();
    }
}
