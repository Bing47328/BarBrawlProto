using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public Text timeText;
    void Start()
    {
        timeText.text = "Timer: " + InGameTimer.timeSpent;
    }
}
