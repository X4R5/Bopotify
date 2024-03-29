﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreRunUpgradeMenu : MonoBehaviour
{
    int _moneyCount;
    [SerializeField] TMPro.TextMeshProUGUI _moneyText;
    [SerializeField] TMPro.TextMeshProUGUI _maxHealthText;
    [SerializeField] TMPro.TextMeshProUGUI _perfectAttackDmgText;
    [SerializeField] TMPro.TextMeshProUGUI _goodAttackDmgText;
    [SerializeField] TMPro.TextMeshProUGUI _perfectAttackRangeText;
    [SerializeField] TMPro.TextMeshProUGUI _goodAttackRangeText;

    [Space(10)]
    [SerializeField] Button _maxHealthButton;
    [SerializeField] Button _perfectAttackDmgButton;
    [SerializeField] Button _goodAttackDmgButton;
    [SerializeField] Button _perfectAttackRangeButton;
    [SerializeField] Button _goodAttackRangeButton;
    [SerializeField] Button _backButton;
    [SerializeField] Button _playButton;

    [Space(10)]
    [SerializeField] int _maxHealthCost;
    [SerializeField] int _perfectAttackDmgCost;
    [SerializeField] int _goodAttackDmgCost;
    [SerializeField] int _perfectAttackRangeCost;
    [SerializeField] int _goodAttackRangeCost;

    private void Awake()
    {
        SetDefaultValues();
        SetButtonClicks();

        //PlayerPrefs.SetInt("Money", 0);
        _moneyCount = PlayerPrefs.GetInt("Money");

        UpdateUI();
    }

    private void SetDefaultValues()
    {
        if (PlayerPrefs.GetInt("MaxHealth") == 0)
        {
            PlayerPrefs.SetInt("MaxHealth", 2);
        }

        if (PlayerPrefs.GetFloat("PerfectAttackDmg") == 0)
        {
            PlayerPrefs.SetFloat("PerfectAttackDmg", 15);
        }

        if (PlayerPrefs.GetFloat("GoodAttackDmg") == 0)
        {
            PlayerPrefs.SetFloat("GoodAttackDmg", 10);
        }

        if (PlayerPrefs.GetFloat("PerfectAttackRange") == 0)
        {
            PlayerPrefs.SetFloat("PerfectAttackRange", 0.6f);
        }

        if (PlayerPrefs.GetFloat("GoodAttackRange") == 0)
        {
            PlayerPrefs.SetFloat("GoodAttackRange", 0.4f);
        }
    }

    private void UpdateUI()
    {
        _moneyText.text = "Para: " + _moneyCount.ToString() + "G";
        if(PlayerPrefs.GetInt("MaxHealth") == 10)
        {
            _maxHealthText.text = "Max can: 10";
            _maxHealthButton.GetComponentInChildren<TMP_Text>().text = "MAX";
            _maxHealthButton.interactable = false;
        }else
        {
            _maxHealthText.text = "Max can:" + PlayerPrefs.GetInt("MaxHealth");
            _maxHealthButton.interactable = _moneyCount >= _maxHealthCost;
        }
        
        if(PlayerPrefs.GetFloat("PerfectAttackDmg") >= 100)
        {
            _perfectAttackDmgText.text = "Perfect saldırı hasarı: 100";
            _perfectAttackDmgButton.GetComponentInChildren<TMP_Text>().text = "MAX";
            _perfectAttackDmgButton.interactable = false;
        }else
        {
            _perfectAttackDmgText.text = "Perfect saldırı hasarı: " + PlayerPrefs.GetFloat("PerfectAttackDmg").ToString();
            _perfectAttackDmgButton.interactable = _moneyCount >= _perfectAttackDmgCost;
        }

        if (PlayerPrefs.GetFloat("GoodAttackDmg") >= 90)
        {
            _goodAttackDmgText.text = "Good saldırı hasarı: 90";
            _goodAttackDmgButton.GetComponentInChildren<TMP_Text>().text = "MAX";
            _goodAttackDmgButton.interactable = false;
        }
        else
        {
            _goodAttackDmgText.text = "Good saldırı hasarı: " + PlayerPrefs.GetFloat("GoodAttackDmg").ToString();
            _goodAttackDmgButton.interactable = _moneyCount >= _goodAttackDmgCost;
        }


        if(PlayerPrefs.GetFloat("PerfectAttackRange") >= 1f)
        {
            _perfectAttackRangeText.text = "Perfect saldırı menzili: 1";
            _perfectAttackRangeButton.GetComponentInChildren<TMP_Text>().text = "MAX";
            _perfectAttackRangeButton.interactable = false;
        }
        else
        {
            _perfectAttackRangeText.text = "Perfect saldırı menzili: " + PlayerPrefs.GetFloat("PerfectAttackRange").ToString();
            _perfectAttackRangeButton.interactable = _moneyCount >= _perfectAttackRangeCost;
        }

        
        if(PlayerPrefs.GetFloat("GoodAttackRange") >= 0.9f)
        {
            _goodAttackRangeText.text = "Perfect saldırı menzili: 1";
            _goodAttackRangeButton.GetComponentInChildren<TMP_Text>().text = "MAX";
            _goodAttackRangeButton.interactable = false;
        }else
        {
            _goodAttackRangeText.text = "Good saldırı menzili: " + PlayerPrefs.GetFloat("GoodAttackRange").ToString();
            _goodAttackRangeButton.interactable = _moneyCount >= _goodAttackRangeCost;
        }
    }

    void SetButtonClicks()
    {
        _maxHealthButton.onClick.AddListener(MaxHealthUpgrade);
        _perfectAttackDmgButton.onClick.AddListener(PerfectAttackDmgUpgrade);
        _goodAttackDmgButton.onClick.AddListener(GoodAttackDmgUpgrade);
        _perfectAttackRangeButton.onClick.AddListener(PerfectAttackRangeUpgrade);
        _goodAttackRangeButton.onClick.AddListener(GoodAttackRangeUpgrade);
        _backButton.onClick.AddListener(Back);
        _playButton.onClick.AddListener(Play);
    }

    public void MaxHealthUpgrade()
    {
        _moneyCount -= _maxHealthCost;
        PlayerPrefs.SetInt("MaxHealth", PlayerPrefs.GetInt("MaxHealth") + 1);
        UpdateUI();
    }

    public void PerfectAttackDmgUpgrade()
    {
        _moneyCount -= _perfectAttackDmgCost;
        PlayerPrefs.SetFloat("PerfectAttackDmg", PlayerPrefs.GetFloat("PerfectAttackDmg") + 5);
        UpdateUI();
    }

    public void GoodAttackDmgUpgrade()
    {
        if (PlayerPrefs.GetFloat("GoodAttackDmg") + 5 >= PlayerPrefs.GetFloat("PerfectAttackDmg"))
        {
            Debug.Log("Good attack dmg can't be bigger than perfect attack dmg");
            return;
        }

        _moneyCount -= _goodAttackDmgCost;
        PlayerPrefs.SetFloat("GoodAttackDmg", PlayerPrefs.GetFloat("GoodAttackDmg") + 5);
        UpdateUI();
    }

    public void PerfectAttackRangeUpgrade()
    {
        _moneyCount -= _perfectAttackRangeCost;
        PlayerPrefs.SetFloat("PerfectAttackRange", PlayerPrefs.GetFloat("PerfectAttackRange") + 0.05f);
        UpdateUI();
    }

    public void GoodAttackRangeUpgrade()
    {
        if (PlayerPrefs.GetFloat("GoodAttackRange") + 0.05f >= PlayerPrefs.GetFloat("PerfectAttackRange"))
        {
            Debug.Log("Good attack range can't be bigger than perfect attack range");
            return;
        }

        _moneyCount -= _goodAttackRangeCost;
        PlayerPrefs.SetFloat("GoodAttackRange", PlayerPrefs.GetFloat("GoodAttackRange") + 0.05f);
        UpdateUI();
    }

    public void Back()
    {
        PlayerPrefs.SetInt("Money", _moneyCount);
        Application.Quit();
    }

    public void Play()
    {
        PlayerPrefs.SetInt("Money", _moneyCount);
        PlayerPrefs.SetInt("CurrentHealth", PlayerPrefs.GetInt("MaxHealth"));
        UnityEngine.SceneManagement.SceneManager.LoadScene("Run");
    }
}
