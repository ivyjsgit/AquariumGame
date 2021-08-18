using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FishManager : Singleton<FishManager>
{
    public Dictionary<FishType, int> FishCounts = new Dictionary<FishType, int>();

    // Use this for initialization
    void Start()
    {
        foreach (FishType fishType in Enum.GetValues(typeof(FishType)))
        {
            //Debug.Log(fishType);
            if (!FishCounts.ContainsKey(fishType))
            {
                FishCounts.Add(fishType, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddFish(FishType fishType)
    {
        if (FishCounts.ContainsKey(fishType))
        {
            FishCounts[fishType] += 1;
        }
        else
        {
            FishCounts.Add(fishType, 1);
        }
    }

    public void RemoveFish(FishType fishType)
    {
        FishCounts[fishType] -= 1;
    }

    public int GetCount(FishType fishType)
    {
        return FishCounts[fishType];
    }
}
