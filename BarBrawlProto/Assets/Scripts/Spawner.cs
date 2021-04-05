using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{ 
    public static int spawn = 10;
    public static int left = 10;

    Text enemyText;

    public GameObject[] prefabs = new GameObject[4];
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    public GameObject win;
    public AudioSource spawned;

    void Reset()
    {
        spawn = 10;
        left = 10;
    }

    private void Start()
    {
        Reset();
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
        enemyText = GameObject.Find("Number").GetComponent<Text>();
    }

    private void Update()
    {
        enemyText.text = left.ToString();
        if (left == 0)
        {
            win.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void SpawnObject()
    {
        if (spawn != 0)
        {
            Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, transform.rotation);
            spawned.Play();
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
