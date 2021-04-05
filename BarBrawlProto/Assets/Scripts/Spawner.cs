using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static int spawn = 5;

    public GameObject[] prefabs = new GameObject[4];
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);

    }

    void SpawnObject()
    {
        if (spawn != 0)
        {
            Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, transform.rotation);
            spawn--;
        }
        else
        {
            stopSpawning = true;
        }

        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
