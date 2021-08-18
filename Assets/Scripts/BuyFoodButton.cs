using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class BuyFoodButton : MonoBehaviour
{
    // Use this for initialization
    public Food food;
    public int cost = 0;
    public TextMeshProUGUI CostLabel;
    private Button button;




    void Start()
    {
        button = GetComponent<Button>();
        CostLabel.text = $"${cost}";
    }

    // Update is called once per frame
    void Update()
    {
        if (MoneyManager.Instance.GetMoney() >= cost && !MouseController.Instance.AlreadyHasFood(food))
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void OnClick()
    {
        Debug.Log("yo");
        if (MoneyManager.Instance.GetMoney() >= cost)
        {
            MoneyManager.Instance.SetMoney(MoneyManager.Instance.GetMoney() - cost);
            MouseController.Instance.ObtainFood(food);
        }
    }
}
