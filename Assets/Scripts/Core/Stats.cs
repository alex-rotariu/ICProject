using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Stats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI moneyPerSecondText;
    PlayerSession session;

    private Player playerStats;
    private int buttonMoney = 1;

    private float timePassedSinceLastSecondUpdate = 0.0f;

    private void Start()
    {
        playerStats = ScenesData.GetPlayer();
        session = FindObjectOfType<PlayerSession>();
    }

    // Update is called once per frame
    void Update()
    {
        //Add the time from the last update to the time passed and check if more than a second passed
        timePassedSinceLastSecondUpdate += Time.deltaTime;
        if (timePassedSinceLastSecondUpdate >= 1.0f)
        {
            //If more than a second passed then reset time passed and add moneyPerSecond to total money
            timePassedSinceLastSecondUpdate = 0.0f;

            AddMoney(playerStats.moneyPerSecond);
        }

        moneyText.text = playerStats.money.ToString();
        moneyPerSecondText.text = playerStats.moneyPerSecond.ToString();
        UpdateDatabase();
    }

    private void UpdateDatabase()
    {
        ScenesData.SetPlayer(playerStats);
        session.SaveToDatabase();

    }

    public void AddMoneyButton()
    {
        AddMoney(buttonMoney);
    }

    public void AddMoney(int amount) {
        playerStats.money += amount;
        
    }

    public void BuyUpgrade(string costAndMPSIncrease)
    {
        int cost, moneyPerSecondIncrease;
        cost = int.Parse(costAndMPSIncrease.Substring(0, 10));
        moneyPerSecondIncrease = int.Parse(costAndMPSIncrease.Substring(13, 10));
        if(costAndMPSIncrease[11]!='0')
        {
            moneyPerSecondIncrease *= -1;
        }
        if (playerStats.money >= cost&&playerStats.moneyPerSecond+moneyPerSecondIncrease>=0)
        {
            playerStats.money -= cost;
            playerStats.moneyPerSecond += moneyPerSecondIncrease;
        }
    }
}
