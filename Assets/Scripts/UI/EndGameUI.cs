using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndGameUI : MonoBehaviour
{
    private GameManager gameManager;
    private CourseManager courseManager;

    private VisualElement root;

    private TextElement bestTime;
    private TextElement currentTime;
    private TextElement bossTimer;

    private InputReader input;

    private void OnEnable()
    {
        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        bestTime = root.Q<TextElement>("UI_Endscreen_BestTime_Text");
        currentTime = root.Q<TextElement>("UI_Endscreen_CurrentTime_Text");
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

    public void DisplayStats(float timeSpent, bool completionStatus)
    {
        var _bestTime = courseManager.GetCurrentCourseBestTime();

        if(_bestTime > 0)
        {
            bestTime.text = "Best Time: " + DisplayTime(_bestTime);
        }
        else
        {
            bestTime.text = "Best Time: --:--:---";
        }

        if (completionStatus)
        {
            if (_bestTime > courseManager.GetCurrentParTime())
            {
                currentTime.style.color = Color.red;
            }
            else
            {
                if (_bestTime < courseManager.GetCurrentParTime())
                   currentTime.style.color = Color.green;
                else
                {
                    currentTime.style.color = Color.black;
                }
            }
        }
        else
            currentTime.style.color = Color.red;

        currentTime.text = "Current Time: " + DisplayTime(timeSpent);
        bossTimer.text = "Boss Timer: " + DisplayTime(courseManager.GetCurrentBossTimeLimit());
    }

    private void Submit()
    {
        // Save the data
        GameManager.instance.dataManager.SaveData(courseManager.GetSetData());

        // Load CourseSelectionMenu
        SceneManager.LoadScene(0);

        gameObject.SetActive(false);
    }

    // Converts a float time in seconds to a 00:00 formatted string
    private string DisplayTime(float timeInSeconds)
    {
        var minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        var seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        var milliSeconds = Mathf.FloorToInt((timeInSeconds % 1) * 1000);
        return $"{minutes:00}:{seconds:00}:{milliSeconds:000}";
    }
}
