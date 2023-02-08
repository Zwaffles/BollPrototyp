using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerUIController : MonoBehaviour
{
    TextElement minutes;
    TextElement seconds;
    TextElement milliSeconds;


    private void OnEnable()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        minutes = root.Q<TextElement>("Minutes");
        seconds = root.Q<TextElement>("Seconds");
        milliSeconds = root.Q<TextElement>("Milliseconds");
    }

    public void UpdateTimerUI(float timeSpent)
    {
        minutes.text = Mathf.FloorToInt(timeSpent / 60f).ToString();
        seconds.text = Mathf.FloorToInt(timeSpent % 60f).ToString();
        milliSeconds.text = Mathf.FloorToInt((timeSpent % 1) * 1000).ToString();
    }
}
