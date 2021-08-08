using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyFishButton : MonoBehaviour
{
    // Use this for initialization
    public GameObject fish;
    public int cost;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MoneyManager.Instance.GetMoney() >= cost)
        {
            button.interactable = true;
        }
        else {
            button.interactable = false;
        }
    }

   public void OnClick()
    {
        Vector3 PositionToSpawn = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        Instantiate(fish);

    }
}
