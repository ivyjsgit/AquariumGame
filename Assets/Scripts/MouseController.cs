using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : Singleton<MouseController>
{
    // Start is called before the first frame update

    public GameObject food;
    public List<String> TagsExcludedFromFoodSpawning = new List<String>();

    void Start()
    {
        Debug.Log(Camera.main);
        Debug.Log(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;       // we want 2m away from the camera position
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(objectPos, Vector2.zero);
            Debug.DrawRay(objectPos, Vector3.zero, Color.red);
            
            if (hit.collider == null)
            {
                Instantiate(food, objectPos, Quaternion.identity);

                //if (!TagsExcludedFromFoodSpawning.Contains(hit.collider.tag))
                //{
                //}
            }
        }
    }
}
