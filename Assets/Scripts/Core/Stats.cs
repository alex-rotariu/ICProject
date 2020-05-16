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

    private ulong money = 0;
    private ulong moneyPerSecond = 0;
    private ulong moneyPerClick = 1;

    private float timePassedSinceLastSecondUpdate = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
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

            money += moneyPerSecond;
        }


        moneyText.text = money.ToString();
        moneyPerSecondText.text = moneyPerSecond.ToString();
    }

    public void loadSaveData()
    {
        DateTime SaveTime = new DateTime();
        List<string> SaveData = new List<string>();
        using (StreamReader sr = new StreamReader("Assets/Resources/SaveData.csv"))
        {
            while (sr.Peek() >= 0)
            {
                SaveData.Add(sr.ReadLine());
            }
        }
        string[] row = SaveData[1].Split(new char[] { ',' });
        money = ulong.Parse(row[0]);
        moneyPerClick = ulong.Parse(row[1]);
        moneyPerSecond = ulong.Parse(row[2]);
        SaveTime = DateTime.Parse(row[3]);
        int PassedTime = (int)(System.DateTime.UtcNow - SaveTime).TotalSeconds;
        addMoney((ulong)PassedTime * moneyPerSecond);

    }
    public void writeSaveData()
    {
        List<string> SaveData = new List<string>();
        using (StreamReader sr = new StreamReader("Assets/Resources/SaveData.csv"))
        {
            while (sr.Peek() >= 0)
            {
                SaveData.Add(sr.ReadLine());
            }
        }
        StreamWriter writer = new StreamWriter("Assets/Resources/SaveData.csv", false);
        writer.WriteLine(SaveData[0]);
        SaveData[1] = money.ToString() + ',' + moneyPerClick.ToString() + ',' + moneyPerSecond.ToString() + ',' + System.DateTime.UtcNow.ToString();
        writer.WriteLine(SaveData[1]);
        writer.Close();
    }

    public ulong getMoney()
    {
        return money;
    }
    public ulong getMoneyPerSecond()
    {
        return moneyPerSecond;
    }
    public ulong getMoneyPerClick()
    {
        return moneyPerClick;
    }

    public void addMoney() 
    {
        money+=moneyPerClick;
        writeSaveData();
    }
    public void addMoney(ulong addedMoney)
    {
        money += addedMoney;
        writeSaveData();
    }
    public void addMoneyPerSecond(ulong addedMPS)
    {
        moneyPerSecond += addedMPS;
        writeSaveData();
    }
    public void addMoneyPerClick(ulong addedMPC)
    {
        moneyPerClick += addedMPC;
        writeSaveData();
    }
    public void addPercentMoneyPerSecond(ulong percentageMPS)
    {
        moneyPerSecond *= (100 + percentageMPS) / 100;
        writeSaveData();
    }
    public void addPercentMoneyPerClick(ulong percentageMPC)
    {
        moneyPerClick *= (100 + percentageMPC) / 100;
        writeSaveData();
    }
    public void removeMoney(ulong removedMoney)
    {
        money -= removedMoney;
        writeSaveData();
    }
    public void removeMoneyPerSecond(ulong removedMPS)
    {
        moneyPerSecond -= removedMPS;
        writeSaveData();
    }
    public void removeMoneyPerClick(ulong removedMPC)
    {
        moneyPerClick -=removedMPC;
        writeSaveData();
    }
}
