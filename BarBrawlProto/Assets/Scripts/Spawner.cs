using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{ 
    public static int spawn = 10;
    public static int killed = 0;

    public GameObject[] prefabs = new GameObject[4];
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    public GameObject win;

    void Reset()
    {
        spawn = 10;
        killed = 0;
    }

    private void Start()
    {
        Reset();
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
            if (killed == 10)
            {
                Cursor.lockState = CursorLockMode.None;
                win.SetActive(true);
                
            }
        }
    }
}
