using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    // Start is called before the first frame update

    public int value;
    public AudioSource PickupSound;

    private void OnMouseDown()
    {
        Collect();
    }

    public void Collect()
    {
        MoneyManager.Instance.SetMoney(MoneyManager.Instance.GetMoney() + value);
        PlaySound();
        StartCoroutine(DestroyMoney());
    }

    public IEnumerator DestroyMoney()
    {
        //Fixes the bug with food spawning on money.
        //Occurred because the money would despawn before the next run of MouseController's Update() function.
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    protected void PlaySound()
    {
        if (PickupSound != null)
        {
            PickupSound.Play();
        }
    }

    public static implicit operator GameObject(Money v)
    {
        throw new NotImplementedException();
    }
}
