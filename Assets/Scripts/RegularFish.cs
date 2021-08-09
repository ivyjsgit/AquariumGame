using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System;

public class RegularFish : Fish
{
    // Use this for initialization

    //public bool isFacingLeft = true;
    //public float minWaitTime = 0.5f;
    //public float maxWaitTime = 10f;

    public float minCoinWaitTime = 3.0f;
    public float maxCoinWaitTime = 10.0f;

    //public Rigidbody2D rigidbody2D;


    //private Vector3 previousPosition = Vector3.zero;

    //new void Start()
    //{
    //    rigidbody2D = GetComponent<Rigidbody2D>();
    //    base.Start();
    //}

    //new void Update()
    //{
    //    //if (ShouldTurnAround())
    //    //{
    //    //    isFacingLeft = !isFacingLeft;
    //    //}
    //    ////MoveFish();
    //    //base.Update();
    //}
    //protected override IEnumerator SelectDirection()
    //{
    //    for(; ; )
    //    {
    //        if (!ShouldTurnAround())
    //        {
    //            isFacingLeft = (Random.value > 0.5);
    //        }
    //        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

    //    }
    //}

    //protected override IEnumerator DropCoins()
    //{
    //    throw new System.NotImplementedException();
    //}

    protected override IEnumerator DropCoins()
    {
        for (; ; )
        {
            List<Money> CoinsToSpawn = new List<Money>();
            yield return new WaitForSeconds(Random.Range(minCoinWaitTime, maxCoinWaitTime));

            var Score = happiness + Random.Range(-5, 50);

            Debug.Log($"Score{Score}");

            //Spawn bronze coin
            if (Score >= 130)
            {
                CoinsToSpawn.Add(MoneyManager.Instance.Denominations[2]);
            }
            else if (Score >= 110)
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

            //CoinsToSpawn.Select(coin => Instantiate(coin, gameObject.transform, true));


            Debug.Log(CoinsToSpawn.Count);

            Debug.Log("Yolo");
        }
        //yield return new WaitForEndOfFrame();
    }
}
