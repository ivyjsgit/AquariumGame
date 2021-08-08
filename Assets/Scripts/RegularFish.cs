using UnityEngine;
using System.Collections;
//using System;

public class RegularFish : Fish
{
    // Use this for initialization

    public bool isFacingLeft = true;
    public float minWaitTime = 0.5f;
    public float maxWaitTime = 10f;

    private Vector3 previousPosition = Vector3.zero;



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
                previousPosition = transform.position;

                transform.position = (newPosition);
            }
    }

    new void Update()
    {
        if (ShouldTurnAround())
        {
            isFacingLeft = !isFacingLeft;
        }
        base.Update();
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

    protected override void ControlSpriteFacing()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //Flip sprite
        if (GetFacing() == Vector3.right)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    private Vector3 GetFacing()
    {
        Vector3 currentDirection = (transform.position - previousPosition).normalized;

        if(currentDirection.x <= 0)
        {
            return Vector3.left;
        }
        else
        {
            return Vector3.right;
        }
    }
}
