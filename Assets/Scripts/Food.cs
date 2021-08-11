using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called before the first frame update

    public int HealthRestored;
    public float TimeAlive = 0.0f;
    public float DespawnTime = 5.0f;
    public float GrowthBonus = 0.0f;

    private float StartTime;




    void Start()
    {

    }

    private void Awake()
    {
        StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        TimeAlive = Time.time - StartTime;
        Despawn();
    }

    protected void Despawn()
    {
        if (TimeAlive >= DespawnTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            Fish fish = collision.gameObject.GetComponent<Fish>();
            fish.FoodTimer += GrowthBonus;
        }
    }

}
