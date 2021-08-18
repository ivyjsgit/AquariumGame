using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BuyFishButton : MonoBehaviour
{
    // Use this for initialization
    public GameObject fish;
    public int cost = 0;
    public int MaxFish = 5;
    public TextMeshProUGUI CostLabel;
    public FishType FishType;
    private Button button;




    void Start()
    {
        button = GetComponent<Button>();
        CostLabel.text = $"${cost}";
    }

    // Update is called once per frame
    void Update()
    {
        if (MoneyManager.Instance.GetMoney() >= cost && FishManager.Instance.GetCount(FishType) < MaxFish)
        {
            button.interactable = true;
        }
        else {
            button.interactable = false;
        }
    }

   public void OnClick()
    {
        if (MoneyManager.Instance.GetMoney() >= cost && FishManager.Instance.GetCount(FishType) < MaxFish)
        {
            Vector3 PositionToSpawn = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            MoneyManager.Instance.SetMoney(MoneyManager.Instance.GetMoney() - cost);
            Instantiate(fish);
        }
    }
}
