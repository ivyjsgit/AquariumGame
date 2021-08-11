using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System;

public class RegularFish : Fish
{
    // Use this for initialization
    public float minCoinWaitTime = 3.0f;
    public float maxCoinWaitTime = 10.0f;
    public List<Sprite> GrowthSprites;

    private void Update()
    {
        LevelUp();
        base.Update();
    }

    protected override IEnumerator DropCoins()
    {
        for (; ; )
        {
            if (GrowthStage != 0)
            {
                List<Money> CoinsToSpawn = new List<Money>();
                var Score = happiness + Random.Range(-5, 50);

                if(Score>=130 && GrowthStage == 2)
                {
                    CoinsToSpawn.Add(MoneyManager.Instance.Denominations[2]);
                }else if (Score >= 110)
                {
                    CoinsToSpawn.Add(MoneyManager.Instance.Denominations[1]);
                }
                else if (Score >= 75)
                {
                    CoinsToSpawn.Add(MoneyManager.Instance.Denominations[0]);
                }

                foreach (Money coin in CoinsToSpawn)
                {
                    Vector3 PositionToSpawn = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                    Instantiate(coin, PositionToSpawn, transform.rotation);
                }
            }

            //CoinsToSpawn.Select(coin => Instantiate(coin, gameObject.transform, true));


            yield return new WaitForSeconds(Random.Range(minCoinWaitTime, maxCoinWaitTime));
        }
    }
    private void LevelUp()
    {
        if (FoodTimer >= 900 && GrowthStage == 1)
        {
            Debug.Log("Level 3!");
            SetLevel(2);
        }
        else if (FoodTimer >= 60 && GrowthStage == 0)
        {
            Debug.Log("Level 2");
            SetLevel(1);
        }
    }



    private void SetLevel(int level)
    {
        //Debug.Log($"Setting level to ")

        GrowthStage = level;
        spriteRenderer.sprite = GrowthSprites[level];

        //Refresh collider
        Destroy(GetComponent<CapsuleCollider2D>());
        gameObject.AddComponent<CapsuleCollider2D>();
    }
}
