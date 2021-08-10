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

    protected override IEnumerator MoveToAndEat()
    {
        for (; ; )
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

            yield return new WaitForSeconds(Random.Range(minEatingTime, maxEatingTime));
        }
    }

    private Money FindNearestMoney(List<GameObject> gameObjectsList)
    {

        List<Money> foodList = gameObjectsList.Select(go => go.GetComponent<Money>()).ToList();
        List<Money> PreferredFoodList = foodList.ToList(); //Clone the list
        List<GameObject> PrefFoodListAsGO = PreferredFoodList.Select(food => food.gameObject).ToList();

        return FindNearestGameObject(PrefFoodListAsGO).GetComponent<Money>();

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
