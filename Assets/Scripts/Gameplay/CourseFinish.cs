using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CourseFinish : MonoBehaviour
{
    private float startTime;
    private float timeSpent;
    private float totalTimeSpent;

    private bool isBossCourse;

    [SerializeField]
    private TimerUIController timerUIController;

    private void Start()
    {
        startTime = Time.time;

        // Get the total time spent for this set
        totalTimeSpent = GameManager.instance.courseManager.GetCurrentSetTotalTimeSpent();
        isBossCourse = GameManager.instance.courseManager.GetCurrentCourseIsBossCourse();
    }

    private void Update()
    {
        timeSpent = Time.time - startTime;

        try
        {
            if (isBossCourse)
            {
                timerUIController.UpdateTimerUI(totalTimeSpent - timeSpent);
            }
            else
            {
                timerUIController.UpdateTimerUI(timeSpent);
            }
        }
        catch
        {
            Debug.Log("Timer UI Controller not found!");
        }

        if (!isBossCourse)
            return;

        // Check if the total time spent has reached 0
        if (totalTimeSpent - timeSpent <= 0)
        {
            // Update the course data as not completed with 0 time spent
            GameManager.instance.courseManager.UpdateCourseData(false, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the course data as completed and update the time spent
            GameManager.instance.courseManager.UpdateCourseData(true, timeSpent);
        }
    }
}
