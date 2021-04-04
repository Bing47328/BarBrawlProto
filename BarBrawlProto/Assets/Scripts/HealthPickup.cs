using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 25;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            FPController.instance.AddHealth(healAmount);
            Destroy(gameObject);
        }
    }
}
