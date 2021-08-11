using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    // Start is called before the first frame update

    //protected Rigidbody2D fishRigidBody;



    [SerializeField] public FoodType PreferredFood = FoodType.Regular;


    public float speed = 3.0f;


    public float FoodTimer = 0.0f;
    public int GrowthStage = 0;

    float timeToReachTarget;

    public int happiness = 100;
    public Rigidbody2D rigidbody2D;

    //Movement based stuff
    public bool isFacingLeft = true;

    //Um, Vector3's can't be nullable, so you have to use nullable syntax for this.
    protected Vector3? previousPosition;


    protected bool MovingTowardsFood = false;
    public List<GameObject> FoodList = new List<GameObject>();
    protected SpriteRenderer spriteRenderer;

    public int EatingRadius = 20;
    public int MinEatingRadius = 1;

    public float minWaitTime;
    public float maxWaitTime;

    protected void Start()
    {
        previousPosition = null;

        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(SelectDirection());

        //StartCoroutine(MoveToAndEat());
        if(PreferredFood != FoodType.Cannibal && PreferredFood !=FoodType.Money)
        {
            StartCoroutine(DropCoins());
        }
    }

    // Update is called once per frame
    protected void Update() {

        FoodTimer += Time.deltaTime;

        ControlSpriteFacing();
        if (ShouldTurnAround())
        {
            isFacingLeft = !isFacingLeft;
        }
    }

    private void FixedUpdate()
    {
        MoveFish();
        MoveToFood();
    }



    protected virtual void MoveFish()
    {
        if (rigidbody2D != null && !MovingTowardsFood)
        {
            var CurrentPosition = transform.position;
            Vector2 newPosition = CurrentPosition;
            if (isFacingLeft)
            {
                newPosition.x += (-Vector3.right * speed * Time.deltaTime).x;
            }
            else
            {
                newPosition.x += (Vector3.right * speed * Time.deltaTime).x;
            }
            previousPosition = CurrentPosition;

            transform.position = (newPosition);
        }
    }



    protected  virtual IEnumerator SelectDirection()
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

    protected virtual void MoveToFood()
    {
        FoodList = GameObject.FindGameObjectsWithTag("Food").ToList();
        if (FoodList.Count > 0)
        {
            Food nearestFood = FindNearestPreferredFood(FoodList);
            if (!MovingTowardsFood)
            {
                StartCoroutine(MoveToTarget(nearestFood.gameObject, 2.0f));
            }
        }
    }

    //This needs to be fixed
    protected virtual IEnumerator MoveToTarget(GameObject OurGameObject, float distance)
    {
        MovingTowardsFood = true;
        float ObjectDistance = GetGameObjectDistance(OurGameObject);
        //Debug.Log(ObjectDistance);

        while (OurGameObject != null &&  ObjectDistance < EatingRadius && ObjectDistance > MinEatingRadius)
        {
            float step = speed * Time.deltaTime;
            previousPosition = transform.position;

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

    protected GameObject FindNearestGameObject(List<GameObject> gameObjects)
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
            if (isFacingLeft)
            {
                return Vector3.left;
            }
            else
            {
                return Vector3.right;
            }
        }

    }

    public void EatFood(float TimeBonus)
    {
        FoodTimer += TimeBonus;
    }

    protected abstract IEnumerator DropCoins();
}
