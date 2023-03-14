using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    TextElement ui_Timer;

    private float timeSpent = 0;

    private bool isRunning = false;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        ui_Timer = root.Q<TextElement>("UI_Timer");
    }

    private void Update()
    {
        if (isRunning)
        {
            timeSpent += Time.deltaTime;
            UpdateTimerUI(timeSpent);
        }
    }

    public void UpdateTimerUI(float timeSpent)
    {
        ui_Timer.text = DisplayTime(timeSpent);
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timeSpent = 0f;
        UpdateTimerUI(timeSpent);
    }

    // Converts a float time in seconds to a 00:00 formatted string
    private string DisplayTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliSeconds = Mathf.FloorToInt((timeInSeconds % 1) * 1000);
        return $"{minutes:00}:{seconds:00}:{milliSeconds:000}";
    }

    public float GetTimeSpent()
    {
        return timeSpent;
    }
}
