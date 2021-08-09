using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piranha : Fish
{

    //Is turned on when in "Chase Mode"
    public bool ChaseModeEnabled = false;
    public float minEatingTime = 5.0f;
    public float maxEatingTime = 15.0f;



    protected override IEnumerator DropCoins()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator MoveToAndEat()
    {
        for (; ; )
        {
            ChaseModeEnabled = true;
            FoodList = GameObject.FindGameObjectsWithTag("Fish").ToList();
            if (FoodList.Count > 0)
            {
                Fish nearestFood = FindNearestFish(FoodList);
                if (!MovingTowardsFood)
                {
                    StartCoroutine(MoveToTarget(nearestFood.gameObject, 2.0f));
                }

            }

            yield return new WaitForSeconds(Random.Range(minEatingTime, maxEatingTime));
        }
    }

    private Fish FindNearestFish(List<GameObject> gameObjectsList)
    {

        List<Fish> foodList = gameObjectsList.Select(go => go.GetComponent<Fish>()).ToList();
        List<Fish> PreferredFoodList = foodList.ToList(); //Clone the list


        foreach (Fish fish in foodList)
        {
            if(fish.PreferredFood == FoodType.Cannibal)
            {
                    PreferredFoodList.Remove(fish);
            }

        }
        List<GameObject> PrefFoodListAsGO = PreferredFoodList.Select(food => food.gameObject).ToList();
        return FindNearestGameObject(PrefFoodListAsGO).GetComponent<Fish>();

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Fish"))
        {
            Fish foodObject = collision.gameObject.GetComponent<Fish>();
            if (foodObject.PreferredFood != FoodType.Cannibal && ChaseModeEnabled == true)
            {
                ChaseModeEnabled = false;
                DropMoneyOnEat();
                Destroy(collision.gameObject);
            }
        }
    }

    public void DropMoneyOnEat()
    {
        Money DollarBill = MoneyManager.Instance.Denominations[2];
        Vector3 PositionToSpawn = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        Instantiate(DollarBill, PositionToSpawn, transform.rotation);

    }
}
