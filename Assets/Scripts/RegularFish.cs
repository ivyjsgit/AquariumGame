using UnityEngine;
using System.Collections;

public class RegularFish : Fish
{
    // Use this for initialization

    public bool isFacingLeft = true;
    public float minWaitTime = 0.5f;
    public float maxWaitTime = 10f;



    protected override void MoveFish()
    {
            if (rigidbody != null && !MovingTowardsFood)
            {
                Vector2 newPosition = transform.position;
                if (isFacingLeft)
                {
                    newPosition.x += (-Vector3.right * speed * Time.deltaTime).x;
                }
                else
                {
                    newPosition.x += (Vector3.right * speed * Time.deltaTime).x;
                }

                transform.position = (newPosition);
            }
    }

    new void Update()
    {
        if (ShouldTurnAround())
        {
            isFacingLeft = !isFacingLeft;
        }
        MoveFish();
    }
    protected override IEnumerator SelectDirection()
    {
        for(; ; )
        {
            if (!ShouldTurnAround())
            {
                isFacingLeft = (Random.value > 0.5);
            }
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        }
    }
}
