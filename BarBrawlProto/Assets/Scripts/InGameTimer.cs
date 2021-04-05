using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour
{
    public Text text;
    DateTime startTime;
    public TimeSpan timeElapsed { get; private set; }

    public static TimeSpan timeSpent;


    // Start is called before the first frame update
    void Start()
    {
        startTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        this.timeElapsed = DateTime.Now - startTime;

        timeSpent = timeElapsed;
        text.text = "Timer: " + timeElapsed;

    }
}
