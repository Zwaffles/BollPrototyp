using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Represents a timer specifically for a boss course.
/// </summary>
public class BossTimer : MonoBehaviour
{
    private TextElement bossTimer;
    private float timeLeft = 1;
    private bool isRunning = false;
    private bool textIsRed = false;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Get the UI element for displaying the boss timer
        bossTimer = root.Q<TextElement>("BossTimer");
    }

    private void Update()
    {
        if (isRunning)
        {
            timeLeft -= Time.deltaTime;
            bossTimer.text = UpdateTimerUI(timeLeft);
        }

        // If time left has reached 0
        if (timeLeft <= 0)
        {
            if (textIsRed)
                return;

            bossTimer.style.color = Color.red;
            textIsRed = true;

            // StopTimer();
            // timeLeft = 0;
            // UpdateTimerUI(timeLeft);
            // GameManager.instance.uiManager.EndGameplay(false);
        }
    }

    /// <summary>
    /// Updates the boss timer UI element with the formatted time.
    /// </summary>
    /// <param name="timeSpent">The time remaining.</param>
    /// <returns>The formatted time string.</returns>
    public string UpdateTimerUI(float timeSpent)
    {
        bool isNegative = timeSpent < 0;
        if (isNegative)
            timeSpent *= -1; // Make the time positive for calculation

        int minutes = Mathf.FloorToInt(timeSpent / 60f);
        int seconds = Mathf.FloorToInt(timeSpent % 60f);
        int milliSeconds = Mathf.FloorToInt((timeSpent % 1) * 1000);

        string sign = isNegative ? "-" : ""; // Add a negative sign if necessary

        return $"{sign}{minutes:00}:{seconds:00}:{milliSeconds:000}";
    }

    /// <summary>
    /// Starts the boss timer.
    /// </summary>
    public void StartTimer()
    {
        isRunning = true;
    }

    /// <summary>
    /// Stops the boss timer.
    /// </summary>
    public void StopTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// Resets the boss timer to its initial state.
    /// </summary>
    public void ResetTimer()
    {
        bossTimer.style.color = Color.black;
        timeLeft = GameManager.instance.courseManager.GetCurrentBossTimeLimit();
        UpdateTimerUI(timeLeft);
    }

    /// <summary>
    /// Gets the remaining time on the boss timer.
    /// </summary>
    /// <returns>The remaining time in seconds.</returns>
    public float GetTimeLeft()
    {
        return timeLeft;
    }
}
