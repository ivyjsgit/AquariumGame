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

            Collider2D[] collisions = Physics2D.OverlapCircleAll(gameObject.transform.position, EatingRadius);
            List<Fish> FoundFishList = new List<Fish>();

            foreach(Collider2D collision in collisions)
            {
                if (collision.gameObject.CompareTag("Fish"))
                {
                    Fish fish = collision.gameObject.GetComponent<Fish>();
                    if (ShouldEatFish(fish))
                    {
                        FoundFishList.Add(fish);
                    }
                }
            }

            if (FoundFishList.Count > 0)
            {
                Fish nearestFish = FindNearestFish(FoundFishList);
                if (!MovingTowardsFood)
                {
                    StartCoroutine(MoveToTarget(nearestFish.gameObject, 2.0f));
                }
            }
        }
    }

    private Fish FindNearestFish(List<Fish> fishList)
    {
        List<GameObject> PrefFoodListAsGO = fishList.Select(food => food.gameObject).ToList();
        PrefFoodListAsGO = PrefFoodListAsGO.FindAll(go => go != null);
        return FindNearestGameObject(PrefFoodListAsGO).GetComponent<Fish>();

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Fish"))
        {
            Fish foodObject = collision.gameObject.GetComponent<Fish>();
            if (ShouldEatFish(foodObject) && ChaseModeEnabled == true)
            {
                ChaseModeEnabled = false;
                DropMoneyOnEat();
                Destroy(collision.gameObject);
            }
        }
    }

    private bool ShouldEatFish(Fish fish)
    {
        if (fish.GetComponent<RegularFish>() != null)
        {
            RegularFish regularFish = fish.GetComponent<RegularFish>();
            if (regularFish.GrowthStage == 0)
            {
                return true;
            }
        }
        return false;
    }

    public void DropMoneyOnEat()
    {
        Money DollarBill = MoneyManager.Instance.Denominations[2];
        Vector3 PositionToSpawn = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        Instantiate(DollarBill, PositionToSpawn, transform.rotation);

    }
}
