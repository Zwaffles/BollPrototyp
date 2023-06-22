using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Manages the UI for the end of the game, displaying statistics and handling user input.
/// </summary>
public class EndGameUI : MonoBehaviour
{
    private GameManager gameManager;
    private CourseManager courseManager;
    private VisualElement root;
    private TextElement bestTime;
    private TextElement currentTime;
    private TextElement parTime;
    private TextElement bossTimer;
    private InputReader input;

    private void OnEnable()
    {
        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        bestTime = root.Q<TextElement>("UI_Endscreen_BestTime_Text");
        currentTime = root.Q<TextElement>("UI_Endscreen_CurrentTime_Text");
        parTime = root.Q<TextElement>("UI_Endscreen_Par-Time_Text");
        bossTimer = root.Q<TextElement>("UI_Endscreen_BossTime_Text");
    }

    private void OnDisable()
    {
        input.RemoveSubmitEventListener(Submit);
    }

    private void Awake()
    {
        gameManager = GameManager.instance;
        courseManager = gameManager.courseManager;
    }

    /// <summary>
    /// Displays the end game statistics on the UI.
    /// </summary>
    /// <param name="timeSpent">The time spent by the player.</param>
    /// <param name="completionStatus">The completion status of the game.</param>
    public void DisplayStats(float timeSpent, bool completionStatus)
    {
        var _bestTime = courseManager.GetCurrentCourseBestTime();
        var _parTime = courseManager.GetCurrentParTime();

        if (_bestTime > 0)
        {
            bestTime.text = "Best Time: " + DisplayTime(_bestTime);
        }
        else
        {
            bestTime.text = "Best Time: --:--:---";
        }

        if (completionStatus)
        {
            if (_bestTime > _parTime)
            {
                currentTime.style.color = Color.red;
            }
            else
            {
                if (_bestTime < _parTime)
                    currentTime.style.color = Color.green;
                else
                {
                    currentTime.style.color = Color.black;
                }
            }
        }
        else
        {
            currentTime.style.color = Color.red;
        }

        currentTime.text = "Current Time: " + DisplayTime(timeSpent);
        parTime.text = "Par-Time: " + DisplayTime(_parTime);
        bossTimer.text = "Boss Timer: " + DisplayTime(courseManager.GetCurrentBossTimeLimit());
    }

    private void Submit()
    {
        // Save the data
        GameManager.instance.dataManager.SaveData(courseManager.GetSetData());

        // Load CourseSelectionMenu
        SceneManager.LoadScene(0);

        GameManager.instance.uiManager.ToggleLevelSelectMenu(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Converts a float time in seconds to a formatted string in the format "00:00:000".
    /// </summary>
    /// <param name="timeInSeconds">The time in seconds to be formatted.</param>
    /// <returns>The formatted time string.</returns>
    private string DisplayTime(float timeInSeconds)
    {
        var minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        var seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        var milliSeconds = Mathf.FloorToInt((timeInSeconds % 1) * 1000);
        return $"{minutes:00}:{seconds:00}:{milliSeconds:000}";
    }
}
