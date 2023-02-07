using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CourseSelectionUI : MonoBehaviour
{
    [SerializeField, Header("The UI buttons for each course")]
    private List<GameObject> courseButtons;
    [SerializeField, Header("Current Set (starts at 0)")]
    private int currentSet = 0;

    private CourseManager courseManager;

    private void Start()
    {
        courseManager = GameManager.instance.courseManager;

        for(int i = 0; i < 5; i++)
        {
            // Lock the course buttons for subcourses 2-5 if the previous subcourse was not completed
            if (i > 0 && !courseManager.GetCompletionStatus(currentSet, i - 1))
            {
                courseButtons[i].GetComponentInChildren<Button>().interactable = false;
                courseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = courseManager.GetCourseName(currentSet, i) + "\nComplete previous course(s) to unlock";
            }
            else
            {
                courseButtons[i].GetComponentInChildren<Button>().interactable = true;

                float timeInSeconds = courseManager.GetTimeSpent(currentSet, i);
                string courseName = courseManager.GetCourseName(currentSet, i);
                courseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = courseName + (timeInSeconds > 0 ? "\nBest Time: " + DisplayTime(timeInSeconds) : "\nCourse not completed");
            }
        }

        if(!courseManager.GetCompletionStatus(currentSet, 4))
        {
            courseButtons[5].GetComponentInChildren<Button>().interactable = false;

            // Assign the boss course's name to the boss course button's text component
            courseButtons[5].GetComponentInChildren<TextMeshProUGUI>().text = courseManager.GetBossCourseName(currentSet) + "\nComplete previous course(s) to unlock";
        }
        else
        {
            courseButtons[5].GetComponentInChildren<Button>().interactable = true;

            float timeInSeconds = courseManager.GetBossTimeSpent(currentSet);
            string courseName = courseManager.GetBossCourseName(currentSet);
            courseButtons[5].GetComponentInChildren<TextMeshProUGUI>().text = courseName + (timeInSeconds > 0 ? "\nBest Time: " + DisplayTime(timeInSeconds) : "\nCourse not completed");
        }

        // Display the remaining time for the boss course
        float bossTime = courseManager.GetBossTimeLimit(currentSet) - courseManager.GetTotalTimeSpent(currentSet);
        courseButtons[5].GetComponentInChildren<TextMeshProUGUI>().text += "\nTime Limit:" + DisplayTime(bossTime);
    }

    public void LoadCourse(int courseIndex)
    {
        courseManager.SetCurrentSet(currentSet);
        courseManager.SetCurrentCourse(courseIndex);
        courseManager.LoadCourse(currentSet, courseIndex);
    }

    public void LoadBossCourse()
    {
        courseManager.SetCurrentSet(currentSet);
        courseManager.SetCurrentCourse(5);
        courseManager.LoadBossCourse(currentSet);
    }

    // Converts a float time in seconds to a 00:00 formatted string
    public string DisplayTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds); 
    }
}
