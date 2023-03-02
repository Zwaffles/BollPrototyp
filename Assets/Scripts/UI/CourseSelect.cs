using System;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void OnEnable()
    {
        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

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
        
        for(int i = 0; i < courseButtons.Length; i++)
        {
            courseButtons[i].RegisterCallback<FocusInEvent>(OnFocusInCourse);
        }
    }

    private void OnDisable()
    {
        input.RemoveSubmitEventListener(Submit);
    }

    private void Start()
    {
        courseManager = GameManager.instance.courseManager;

        currentCourse = courseManager.GetCurrentCourse();

        FocusFirstElement(currentCourse);

        setName.text = courseManager.GetSetName(currentSet);

        // Display the remaining time for the boss course
        var bossTime = courseManager.GetBossTimeLimit(currentSet) - courseManager.GetTotalTimeSpent(currentSet);
        finalCourseTimeLimit.text = DisplayTime(bossTime);
    }

    public void FocusFirstElement(int currentCourse)
    {
        courseButtons[currentCourse].Focus();
    }

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

    private void Submit()
    {
        switch (currentCourse)
        {
            case 0:
                input.RemoveSubmitEventListener(Submit);
                courseManager.SetCurrentSet(currentSet);
                courseManager.SetCurrentCourse(currentCourse);
                courseManager.LoadCourse(currentSet, currentCourse);
                break;
            case int n when n >= 1 && n <= 4 && !courseManager.GetCompletionStatus(currentSet, currentCourse - 1):
                // do nothing if currentCourse is between 1 and 4 and the previous course has not been completed
                break;
            case int n when n >= 1 && n <= 4:
                input.RemoveSubmitEventListener(Submit);
                courseManager.SetCurrentSet(currentSet);
                courseManager.SetCurrentCourse(currentCourse);
                courseManager.LoadCourse(currentSet, currentCourse);
                break;
            case 5 when courseManager.GetCompletionStatus(currentSet, currentCourse - 1):
                input.RemoveSubmitEventListener(Submit);
                courseManager.SetCurrentSet(currentSet);
                courseManager.SetCurrentCourse(currentCourse);
                courseManager.LoadBossCourse(currentSet);
                break;
            case 5:
                // do nothing if the previous course has not been completed
                break;
            default:
                break;
        }
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
