using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    // Start is called before the first frame update

    //protected Rigidbody2D fishRigidBody;



    [SerializeField] protected FoodType PreferredFood = FoodType.Regular;


    public float speed = 3.0f;


    float time;

    float timeToReachTarget;

    public int happiness = 100;
    public Rigidbody2D rigidbody2D;

    //Movement based stuff
    protected bool isFacingLeft = true;

    //Um, Vector3's can't be nullable, so you have to use nullable syntax for this.
    protected Vector3? previousPosition;


    protected bool MovingTowardsFood = false;
    protected List<GameObject> FoodList = new List<GameObject>();
    public int EatingRadius = 20;
    public int MinEatingRadius = 1;

    public float minWaitTime;
    public float maxWaitTime;

    protected void Start()
    {
        previousPosition = null;

        rigidbody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(SelectDirection());

        StartCoroutine(MoveToAndEat());

        StartCoroutine(DropCoins());
    }

    // Update is called once per frame
    protected void Update() {

        time += Time.deltaTime / timeToReachTarget;

        ControlSpriteFacing();
        MoveFish();
        if (ShouldTurnAround())
        {
            isFacingLeft = !isFacingLeft;
        }
    }



    protected void MoveFish()
    {
        if (rigidbody2D != null && !MovingTowardsFood)
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



    protected  IEnumerator SelectDirection()
    {
        for (; ; )
        {
            if (!ShouldTurnAround())
            {
                isFacingLeft = (Random.value > 0.5);
            }
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, EatingRadius);
    }



    protected bool ShouldTurnAround()
    {
        return transform.position.x <= -10.85f || transform.position.x >= 10.85f;
    }

    protected IEnumerator MoveToAndEat()
    {
        for (; ; )
        {
            FoodList= GameObject.FindGameObjectsWithTag("Food").ToList();
            if (FoodList.Count > 0)
            {
                Food nearestFood = FindNearestPreferredFood(FoodList);
                if (!MovingTowardsFood)
                {
                    StartCoroutine(MoveToTarget(nearestFood.gameObject, 2.0f));
                }

            }

            yield return new WaitForEndOfFrame();
        }
    }

    //This needs to be fixed
    protected IEnumerator MoveToTarget(GameObject OurGameObject, float distance)
    {
        MovingTowardsFood = true;
        float ObjectDistance = GetGameObjectDistance(OurGameObject);
        Debug.Log(ObjectDistance);

        while (OurGameObject != null &&  ObjectDistance < EatingRadius && ObjectDistance > MinEatingRadius)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, OurGameObject.transform.position, speed*Time.deltaTime) ;
            yield return new WaitForEndOfFrame();
            ObjectDistance = GetGameObjectDistance(OurGameObject);
        }
        MovingTowardsFood = false;
        yield return new WaitForEndOfFrame();
    }


    protected float GetGameObjectDistance(GameObject g)
    {
        float AbsolutePositionChange = Mathf.NegativeInfinity;
        if (g != null)
        {
            AbsolutePositionChange = Vector2.Distance(gameObject.transform.position, g.transform.position);
        }
        return AbsolutePositionChange;

    }

    private GameObject FindNearestGameObject(List<GameObject> gameObjects)
    {
        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach(GameObject g in gameObjects)
        {
            if(g != null)
            {
                float AbsolutePositionChange = GetGameObjectDistance(g);

                if (AbsolutePositionChange < distance)
                {
                    distance = AbsolutePositionChange;
                    nearest = g;
                }
            }
        }

        return nearest;
    }

    private Food FindNearestPreferredFood(List<GameObject> gameObjects)
    {
        List<Food> foodList= gameObjects.Select(go => go.GetComponent<Food>()).ToList();
        List<Food> PreferredFoodList = foodList.ToList();
        //PreferredFoodList.AddRange(foodList);


        foreach(Food food in foodList)
        {
            if (!IsFoodPreferred(food))
            {
                PreferredFoodList.Remove(food);
            }
        }
        List<GameObject> PrefFoodListAsGO = PreferredFoodList.Select(food => food.gameObject).ToList();
        return FindNearestGameObject(PrefFoodListAsGO).GetComponent<Food>();

    }


    protected bool IsFoodPreferred(Food food)
    {
        FoodType? CurrentFoodType = null;
        if(food is PlainFood)
        {
            CurrentFoodType = FoodType.Regular;
        }
        if (CurrentFoodType != null)
        {
            return CurrentFoodType == PreferredFood;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Food"))       
        {
            Food foodObject = collision.gameObject.GetComponent<Food>();
            if (IsFoodPreferred(foodObject))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    protected void ControlSpriteFacing()
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
        if (previousPosition != null)
        {
            Vector3 currentDirection = (transform.position - previousPosition.Value).normalized;

            if (currentDirection.x <= 0)
            {
                return Vector3.left;
            }
            else
            {
                return Vector3.right;
            }
        }
        else
        {
            return Vector3.left;
        }

    }

    protected abstract IEnumerator DropCoins();
}
