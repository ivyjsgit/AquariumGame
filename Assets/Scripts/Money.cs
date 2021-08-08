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

    protected void Collect()
    {
        MoneyManager.Instance.SetMoney(MoneyManager.Instance.GetMoney() + value);
        PlaySound();
        Destroy(gameObject);

    }

    protected void PlaySound()
    {
        if (PickupSound != null)
        {
            PickupSound.Play();
        }
    }

}
