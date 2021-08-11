using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyEater : Fish
{

    //Is turned on when in "Chase Mode"
    public float minEatingTime = 5.0f;
    public float maxEatingTime = 15.0f;



    protected override IEnumerator DropCoins()
    {
        yield return new WaitForEndOfFrame();
    }

    protected override void MoveToFood()
    {
        FoodList = GameObject.FindGameObjectsWithTag("Money").ToList();
        if (FoodList.Count > 0)
        {
            Money nearestFood = FindNearestMoney(FoodList);
            if (!MovingTowardsFood)
            {
                StartCoroutine(MoveToTarget(nearestFood.gameObject, 2.0f));
            }
        }
    }

    private Money FindNearestMoney(List<GameObject> gameObjectsList)
    {
        List<GameObject> MoneyList = new List<GameObject>();
        foreach(GameObject go in gameObjectsList)
        {
            Money MoneyPart = go.GetComponent<Money>();
            if (MoneyPart != null)
            {
                MoneyList.Add(go);
            }
        }

        return FindNearestGameObject(MoneyList).GetComponent<Money>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Money"))
        {
            Money money = collision.gameObject.GetComponent<Money>();
            money.Collect();
        }
    }
}
