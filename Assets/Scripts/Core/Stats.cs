using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Stats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI moneyPerSecondText;

    private double money = 0.0f;
    private double moneyPerSecond = 0.0f;
    private double moneyPerClick = 1.0f;

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
            money = System.Math.Round(money,2);
        }


        moneyText.text = money.ToString();
        moneyPerSecondText.text = moneyPerSecond.ToString();
    }

    public void loadSaveData()
    {
        List<string> SaveData = new List<string>();
        using (StreamReader sr = new StreamReader("Assets/Resources/SaveData.csv"))
        {
            while (sr.Peek() >= 0)
            {
                SaveData.Add(sr.ReadLine());
            }
        }
        string[] row = SaveData[1].Split(new char[] { ',' });
        money = double.Parse(row[0]);
        moneyPerClick = double.Parse(row[1]);
        moneyPerSecond = double.Parse(row[2]);
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
        SaveData[1] = money.ToString() + ',' + moneyPerClick.ToString() + ',' + moneyPerSecond.ToString();
        writer.WriteLine(SaveData[1]);
        writer.Close();
    }

    public double getMoney()
    {
        return money;
    }
    public double getMoneyPerSecond()
    {
        return moneyPerSecond;
    }
    public double getMoneyPerClick()
    {
        return moneyPerClick;
    }

    public void addMoney() 
    {
        money+=moneyPerClick;
        money = System.Math.Round(money, 2);
        writeSaveData();
    }
    public void addMoney(double addedMoney)
    {
        money += addedMoney;
        money = System.Math.Round(money, 2);
        writeSaveData();
    }
    public void addMoneyPerSecond(double addedMPS)
    {
        moneyPerSecond += addedMPS;
        moneyPerSecond = System.Math.Round(moneyPerSecond, 2);
        writeSaveData();
    }
    public void addMoneyPerClick(double addedMPC)
    {
        moneyPerClick += addedMPC;
        moneyPerClick = System.Math.Round(moneyPerClick, 2);
        writeSaveData();
    }
    public void addPercentMoneyPerSecond(double percentageMPS)
    {
        moneyPerSecond *= (100 + percentageMPS) / 100;
        moneyPerSecond = System.Math.Round(moneyPerSecond, 2);
        writeSaveData();
    }
    public void addPercentMoneyPerClick(double percentageMPC)
    {
        moneyPerClick *= (100 + percentageMPC) / 100;
        moneyPerClick = System.Math.Round(moneyPerClick, 2);
        writeSaveData();
    }
    public void removeMoney(double removedMoney)
    {
        money -= removedMoney;
        money = System.Math.Round(money, 2);
        writeSaveData();
    }
    public void removeMoneyPerSecond(double removedMPS)
    {
        moneyPerSecond -= removedMPS;
        moneyPerSecond = System.Math.Round(moneyPerSecond, 2);
        writeSaveData();
    }
    public void removeMoneyPerClick(double removedMPC)
    {
        moneyPerClick -=removedMPC;
        moneyPerClick = System.Math.Round(moneyPerClick, 2);
        writeSaveData();
    }
}
