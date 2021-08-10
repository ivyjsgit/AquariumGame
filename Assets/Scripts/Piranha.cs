using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piranha : Fish
{

    //Is turned on when in "Chase Mode"
    public bool ChaseModeEnabled = false;
    public int MaxEatingScore = 5;
    public int MaxEatingRNG = 100;
    public List<FoodType> DoNotEatList = new List<FoodType> { FoodType.Cannibal, FoodType.Money };

    protected override IEnumerator DropCoins()
    {
        throw new System.NotImplementedException();
    }

    protected override void MoveToFood()
    {
        int RNGChoice = Random.Range(0, MaxEatingRNG);
        if (RNGChoice <= MaxEatingScore)
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
        }
    }

    private Fish FindNearestFish(List<GameObject> gameObjectsList)
    {

        List<Fish> foodList = gameObjectsList.Select(go => go.GetComponent<Fish>()).ToList();
        List<Fish> PreferredFoodList = foodList.ToList(); //Clone the list


        foreach (Fish fish in foodList)
        {
            if(DoNotEatList.Contains(fish.PreferredFood))
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
            if (!DoNotEatList.Contains(foodObject.PreferredFood) && ChaseModeEnabled == true)
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
