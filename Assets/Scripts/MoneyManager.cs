using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int money = 0;
    [SerializeField] private int MaxMoney = 999999;

    public List<Money> Denominations;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int money)
    {
        if (money < 0)
        {
            throw new System.ArgumentOutOfRangeException("You cannot have negative money!");
        }
        if(this.money + money <= MaxMoney)
        {
            this.money = money;
        }
        else
        {
            this.money = MaxMoney;
        }
    }
}
