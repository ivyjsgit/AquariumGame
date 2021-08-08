using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI ScoreText;

    public string MoneyPrefix = "Money: ";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = $"{MoneyPrefix}{MoneyManager.Instance.GetMoney()}";
    }
}
