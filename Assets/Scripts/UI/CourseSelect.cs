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
        float bossTime = courseManager.GetBossTimeLimit(currentSet) - courseManager.GetTotalTimeSpent(currentSet);
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

        if(currentCourse < 5)
        {
            courseName.text = courseManager.GetCourseName(currentSet, currentCourse);
            courseBestTime.text = "BestTime: " + DisplayTime(courseManager.GetTimeSpent(currentSet, currentCourse));
        }
        else
        {
            courseName.text = courseManager.GetBossCourseName(currentSet);
            courseBestTime.text = "BestTime: " + DisplayTime(courseManager.GetBossTimeSpent(currentSet));
        }
    }

    private void Submit()
    {
        if (currentCourse > 0 && !courseManager.GetCompletionStatus(currentSet, currentCourse - 1))
                return;

        if (currentCourse < 5)
        {
            courseManager.SetCurrentSet(currentSet);
            courseManager.SetCurrentCourse(currentCourse);
            courseManager.LoadCourse(currentSet, currentCourse);
        }
        else
        {
            courseManager.SetCurrentSet(currentSet);
            courseManager.SetCurrentCourse(5);
            courseManager.LoadBossCourse(currentSet);
        }
    }

    // Converts a float time in seconds to a 00:00 formatted string
    private string DisplayTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliSeconds = Mathf.FloorToInt((timeInSeconds % 1) * 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }
}
