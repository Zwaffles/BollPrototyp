using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CourseFinish : MonoBehaviour
{
    private float startTime;
    private float timeSpent;

    [SerializeField]
    private TimerUIController timerUIController;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        timeSpent = Time.time - startTime;

        try
        {
            timerUIController.UpdateTimerUI(timeSpent);
        }
        catch
        {
            Debug.Log("Timer UI Controller not found!");
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

    private void UpdateTimerUIController(float timeSpent)
    {
        string minutes = Mathf.FloorToInt(timeSpent / 60f).ToString();
        string seconds = Mathf.FloorToInt(timeSpent % 60f).ToString();
        string milliSeconds = Mathf.FloorToInt((timeSpent % 1) * 1000).ToString();
    }
}
