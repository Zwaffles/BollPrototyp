using System;
using UnityEngine.UIElements;
using UnityEngine;

/// <summary>
/// Handles the selection of courses in a menu.
/// </summary>
public class CourseSelect : MonoBehaviour
{
    [SerializeField, Header("Current Set (starts at 0)")]
    private int currentSet = 0;
    private int currentCourse = 0;

    private CourseManager courseManager;

    private VisualElement root;

    private TextElement setName;
    private TextElement finalCourseTimeLimit;
    private TextElement courseName;
    private TextElement courseBestTime;

    private Button[] courseButtons = new Button[6];

    private InputReader input;

    [SerializeField, Header("Lock icon for locked levels")]
    private Texture2D lockIcon;
    [SerializeField, Header("Icon for unlocked levels")]
    private Texture2D unlockIcon;

    /// <summary>
    /// Event triggered when the script is enabled.
    /// </summary>
    private void OnEnable()
    {
        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);
        input.AddShoulderButtonRightEventListener(NavigateSetRight);
        input.AddShoulderButtonLeftEventListener(NavigateSetLeft);

        root = GetComponent<UIDocument>().rootVisualElement;

        setName = root.Q<TextElement>("UI_LS_Header_Text");
        finalCourseTimeLimit = root.Q<TextElement>("UI_LS_BossTimer_Text");
        courseName = root.Q<TextElement>("UI_LS_CourseName_Text");
        courseBestTime = root.Q<TextElement>("UI_LS_CourseBestTime_Text");

        courseButtons[0] = root.Q<Button>("UI_LS_Course_1-1");
        courseButtons[1] = root.Q<Button>("UI_LS_Course_1-2");
        courseButtons[2] = root.Q<Button>("UI_LS_Course_1-3");
        courseButtons[3] = root.Q<Button>("UI_LS_Course_1-4");
        courseButtons[4] = root.Q<Button>("UI_LS_Course_1-5");
        courseButtons[5] = root.Q<Button>("UI_LS_Course_1-X");

        for (int i = 0; i < courseButtons.Length; i++)
        {
            courseButtons[i].RegisterCallback<FocusInEvent>(OnFocusInCourse);
        }

        // Taken from void start

        courseManager = GameManager.instance.courseManager;

        currentCourse = courseManager.GetCurrentCourse();

        FocusFirstElement(currentCourse);

        setName.text = courseManager.GetSetName(currentSet);

        // Display the remaining time for the boss course
        var bossTime = courseManager.GetBossTimeLimit(currentSet) - courseManager.GetTotalTimeSpent(currentSet);
        finalCourseTimeLimit.text = DisplayTime(bossTime);

        for (int i = 1; i < 6; i++)
        {
            // Lock the course buttons for subcourses 2-5 if the previous subcourse was not completed
            if (!courseManager.GetCompletionStatus(currentSet, i - 1))
            {
                courseButtons[i].text = "";
                courseButtons[i].style.backgroundImage = lockIcon;
            }
        }

    }

    /// <summary>
    /// Event triggered when the script is disabled.
    /// </summary>
    private void OnDisable()
    {
        input.RemoveSubmitEventListener(Submit);
        input.RemoveShoulderButtonRightEventListener(NavigateSetRight);
        input.RemoveShoulderButtonLeftEventListener(NavigateSetLeft);
    }

    /*

    /// <summary>
    /// Initializes the script.
    /// </summary>
    private void Start()
    {
        courseManager = GameManager.instance.courseManager;

        currentCourse = courseManager.GetCurrentCourse();

        FocusFirstElement(currentCourse);

        setName.text = courseManager.GetSetName(currentSet);

        // Display the remaining time for the boss course
        var bossTime = courseManager.GetBossTimeLimit(currentSet) - courseManager.GetTotalTimeSpent(currentSet);
        finalCourseTimeLimit.text = DisplayTime(bossTime);

        for (int i = 1; i < 6; i++)
        {
            // Lock the course buttons for subcourses 2-5 if the previous subcourse was not completed
            if (!courseManager.GetCompletionStatus(currentSet, i - 1))
            {
                courseButtons[i].text = "";
                courseButtons[i].style.backgroundImage = lockIcon;
            }
        }
    }

    */

    /// <summary>
    /// Sets the focus on the first element of the selected course.
    /// </summary>
    /// <param name="currentCourse">The index of the current course.</param>
    public void FocusFirstElement(int currentCourse)
    {
        courseButtons[currentCourse].Focus();
    }

    /// <summary>
    /// Event triggered when a course button gains focus.
    /// </summary>
    /// <param name="evt">The FocusInEvent.</param>
    private void OnFocusInCourse(FocusInEvent evt)
    {
        var courseButton = evt.target as Button;
        currentCourse = Array.IndexOf(courseButtons, courseButton);

        if (currentCourse < 5)
        {
            ShowCourseInfo(currentSet, currentCourse);
        }
        else
        {
            ShowBossCourseInfo(currentSet);
        }
    }

    /// <summary>
    /// Displays the information for a selected course.
    /// </summary>
    /// <param name="setIndex">The index of the course set.</param>
    /// <param name="courseIndex">The index of the course within the set.</param>
    private void ShowCourseInfo(int setIndex, int courseIndex)
    {
        courseName.text = courseManager.GetCourseName(setIndex, courseIndex);

        var timeSpent = courseManager.GetTimeSpent(setIndex, courseIndex);

        if (timeSpent > 0)
        {
            courseBestTime.text = "BestTime: " + DisplayTime(timeSpent);
        }
        else
        {
            courseBestTime.text = "BestTime: --:--:---";
        }
    }

    /// <summary>
    /// Displays the information for the boss course.
    /// </summary>
    /// <param name="setIndex">The index of the course set.</param>
    private void ShowBossCourseInfo(int setIndex)
    {
        courseName.text = courseManager.GetBossCourseName(setIndex);

        var timeSpent = courseManager.GetBossTimeSpent(setIndex);

        if (timeSpent > 0)
        {
            courseBestTime.text = "BestTime: " + DisplayTime(timeSpent);
        }
        else
        {
            courseBestTime.text = "BestTime: --:--:---";
        }
    }

    /// <summary>
    /// Event triggered when the submit button is pressed.
    /// </summary>
    private void Submit()
    {
        switch (currentCourse)
        {
            case 0:
                input.RemoveSubmitEventListener(Submit);
                courseManager.SetCurrentSet(currentSet);
                courseManager.SetCurrentCourse(currentCourse);
                courseManager.LoadCourse(currentSet, currentCourse);
                gameObject.SetActive(false);
                break;
            case int n when n >= 1 && n <= 4 && !courseManager.GetCompletionStatus(currentSet, currentCourse - 1):
                // do nothing if currentCourse is between 1 and 4 and the previous course has not been completed
                break;
            case int n when n >= 1 && n <= 4:
                input.RemoveSubmitEventListener(Submit);
                courseManager.SetCurrentSet(currentSet);
                courseManager.SetCurrentCourse(currentCourse);
                courseManager.LoadCourse(currentSet, currentCourse);
                gameObject.SetActive(false);
                break;
            case 5 when courseManager.GetCompletionStatus(currentSet, currentCourse - 1):
                input.RemoveSubmitEventListener(Submit);
                courseManager.SetCurrentSet(currentSet);
                courseManager.SetCurrentCourse(currentCourse);
                courseManager.LoadBossCourse(currentSet);
                gameObject.SetActive(false);
                break;
            case 5:
                // do nothing if the previous course has not been completed
                break;
            default:
                break;
        }
    }

    private void NavigateSetRight()
    {
        do
        {
            currentSet = currentSet == courseManager.GetAmountOfSets() - 1 ? 0 : currentSet + 1;
        } 
        while (!courseManager.GetUnlockStatusOfSet(currentSet));
        UpdateCourseUIInformation();
    }

    private void NavigateSetLeft()
    {
        do
        {
            currentSet = currentSet == 0 ? courseManager.GetAmountOfSets() - 1 : currentSet - 1;
        }
        while (!courseManager.GetUnlockStatusOfSet(currentSet));
        UpdateCourseUIInformation();
    }

    public void NavigateToSet(int setNumber)
    {

        if (setNumber >= 0 && setNumber < courseManager.GetAmountOfSets())
        {

            while (!courseManager.GetUnlockStatusOfSet(setNumber))
            {
                setNumber = setNumber == 0 ? courseManager.GetAmountOfSets() - 1 : setNumber - 1;
            }

            currentSet = setNumber;
            UpdateCourseUIInformation();
        }

    }

    private void UpdateCourseUIInformation()
    {
        Debug.Log("Current Set: " + currentSet);

        setName.text = courseManager.GetSetName(currentSet);

        // Display the remaining time for the boss course
        var bossTime = courseManager.GetBossTimeLimit(currentSet) - courseManager.GetTotalTimeSpent(currentSet);
        finalCourseTimeLimit.text = DisplayTime(bossTime);

        for (int i = 1; i < 6; i++)
        {
            // Lock the course buttons for subcourses 2-5 if the previous subcourse was not completed
            if (!courseManager.GetCompletionStatus(currentSet, i - 1))
            {
                courseButtons[i].text = "";
                courseButtons[i].style.backgroundImage = lockIcon;
            }

            // Unlock the course buttons for subcourses 2-5 if the previous subcourse was completed
            if (courseManager.GetCompletionStatus(currentSet, i - 1))
            {
                courseButtons[i].style.backgroundImage = unlockIcon;
            }
        }

        if (currentCourse < 5)
        {
            ShowCourseInfo(currentSet, currentCourse);
        }
        else
        {
            ShowBossCourseInfo(currentSet);
        }
    }

    /// <summary>
    /// Converts a time in seconds to a formatted string in the format 00:00:000.
    /// </summary>
    /// <param name="timeInSeconds">The time in seconds.</param>
    /// <returns>The formatted time string.</returns>
    private string DisplayTime(float timeInSeconds)
    {
        var minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        var seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        var milliSeconds = Mathf.FloorToInt((timeInSeconds % 1) * 1000);
        return $"{minutes:00}:{seconds:00}:{milliSeconds:000}";
    }
}
