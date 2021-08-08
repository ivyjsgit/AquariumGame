using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    private int money = 0;
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
        this.money = money;
    }
}
