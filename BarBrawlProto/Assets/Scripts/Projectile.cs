using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 3;

    public Rigidbody rb;

    private Vector3 direction;

    void Start()
    {
        direction = FPController.instance.transform.position - transform.position;
        direction.Normalize();
        direction = direction * speed;
    }

    void Update()
    {
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FPController.instance.TakeDMG(5);

            Destroy(gameObject);
        }    
    }
}
