using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Represents a timer for tracking time spent.
/// </summary>
public class Timer : MonoBehaviour
{
    private TextElement ui_Timer;
    private float timeSpent = 0;
    private bool isRunning = false;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Get the UI element for displaying the timer
        ui_Timer = root.Q<TextElement>("UI_Timer");
    }

    private void Update()
    {
        if (isRunning)
        {
            // Update the time spent and update the UI
            timeSpent += Time.deltaTime;
            UpdateTimerUI(timeSpent);
        }
    }

    /// <summary>
    /// Updates the UI element with the formatted time.
    /// </summary>
    /// <param name="timeSpent">The time spent to display.</param>
    public void UpdateTimerUI(float timeSpent)
    {
        ui_Timer.text = DisplayTime(timeSpent);
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        isRunning = true;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// Resets the timer to zero and updates the UI.
    /// </summary>
    public void ResetTimer()
    {
        timeSpent = 0f;
        UpdateTimerUI(timeSpent);
    }

    /// <summary>
    /// Converts a float time in seconds to a formatted string (e.g., 00:00:000).
    /// </summary>
    /// <param name="timeInSeconds">The time in seconds to format.</param>
    /// <returns>The formatted time string.</returns>
    private string DisplayTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliSeconds = Mathf.FloorToInt((timeInSeconds % 1) * 1000);
        return $"{minutes:00}:{seconds:00}:{milliSeconds:000}";
    }

    /// <summary>
    /// Gets the current time spent.
    /// </summary>
    /// <returns>The current time spent.</returns>
    public float GetTimeSpent()
    {
        return timeSpent;
    }
}
