using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    private int money = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void addMoney() {
        money++;
        moneyText.text = money.ToString();
    }
}
