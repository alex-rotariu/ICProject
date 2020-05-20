using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class Stats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI moneyPerSecondText;
    PlayerSession session;

    private Player playerStats;

    private float timePassedSinceLastSecondUpdate = 0.0f;

    private void Start()
    {
        playerStats = ScenesData.GetPlayer();
        session = FindObjectOfType<PlayerSession>();
        loadSaveData();
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

            addMoney(playerStats.moneyPerSecond);
        }

        moneyText.text = playerStats.money.ToString();
        moneyPerSecondText.text = playerStats.moneyPerSecond.ToString();
        UpdateDatabase();
    }

    private void UpdateDatabase()
    {
        playerStats.lastSaveTime = System.DateTime.UtcNow.ToString();
        ScenesData.SetPlayer(playerStats);
        session.SaveToDatabase();
    }

    public Player getPlayer()
    {
        return playerStats;
    }

    public void setPlayer(Player player)
    {
        playerStats = player;
    }

    public void loadSaveData()
    {
        ulong PassedTime = (ulong)(System.DateTime.UtcNow - DateTime.Parse(playerStats.lastSaveTime)).TotalSeconds;
        playerStats.lastSaveTime = System.DateTime.UtcNow.ToString();
        addMoney((ulong)PassedTime * playerStats.moneyPerSecond);
    }

    public ulong getMoney()
    {
        return playerStats.money;
    }
    public ulong getMoneyPerSecond()
    {
        return playerStats.moneyPerSecond;
    }
    public ulong getMoneyPerClick()
    {
        return playerStats.moneyPerClick;
    }

    public void addMoney() 
    {
        playerStats.money += playerStats.moneyPerClick;
    }
    public void addMoney(ulong addedMoney)
    {
        playerStats.money += addedMoney;
    }
    public void addMoneyPerSecond(ulong addedMPS)
    {
        playerStats.moneyPerSecond += addedMPS;
    }
    public void addMoneyPerClick(ulong addedMPC)
    {
        playerStats.moneyPerClick += addedMPC;
    }
    public void addPercentMoneyPerSecond(ulong percentageMPS)
    {
        playerStats.moneyPerSecond *= (100 + percentageMPS) / 100;
    }
    public void addPercentMoneyPerClick(ulong percentageMPC)
    {
        playerStats.moneyPerClick *= (100 + percentageMPC) / 100;
    }
    public void removeMoney(ulong removedMoney)
    {
        playerStats.money -= removedMoney;
    }
    public void removeMoneyPerSecond(ulong removedMPS)
    {
        playerStats.moneyPerSecond -= removedMPS;
    }
    public void removeMoneyPerClick(ulong removedMPC)
    {
        playerStats.moneyPerClick -=removedMPC;
    }
}
