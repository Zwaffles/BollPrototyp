using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseFinish : MonoBehaviour
{
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float timeSpent = Time.time - startTime;
            GameManager.instance.courseManager.UpdateCourseData(true, timeSpent);
        }
    }
}
