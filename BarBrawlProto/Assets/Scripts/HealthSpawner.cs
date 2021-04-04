using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    public GameObject spawn;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    void SpawnObject()
    {
        Instantiate(spawn, transform.position, transform.rotation);
        stopSpawning = true;
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }
}