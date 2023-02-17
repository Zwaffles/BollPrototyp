using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    TextElement minutes;
    TextElement seconds;
    TextElement milliSeconds;

    private float timeSpent = 0;

    private bool isRunning = false;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        minutes = root.Q<TextElement>("Minutes");
        seconds = root.Q<TextElement>("Seconds");
        milliSeconds = root.Q<TextElement>("Milliseconds");
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
        minutes.text = Mathf.FloorToInt(timeSpent / 60f).ToString();
        seconds.text = Mathf.FloorToInt(timeSpent % 60f).ToString();
        milliSeconds.text = Mathf.FloorToInt((timeSpent % 1) * 1000).ToString();
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

    public float GetTimeSpent()
    {
        return timeSpent;
    }
}
