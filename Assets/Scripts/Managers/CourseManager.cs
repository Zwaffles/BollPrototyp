using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CourseManager : MonoBehaviour
{
    [SerializeField, Header("A list of SetData objects for each set of courses")]
    private List<SetData> sets;

    private int currentSet;
    private int currentCourse;

    public void LoadCourse(int setIndex, int courseIndex)
    {
        SceneManager.LoadScene(sets[setIndex].subCourses[courseIndex].sceneName);
    }

    public void LoadBossCourse(int setIndex)
    {
        SceneManager.LoadScene(sets[setIndex].bossCourse.sceneName);
    }

    public void LoadPlayerProgress(List<SetData> setDataList)
    {
        sets = setDataList;
    }

    public void UpdateCourseData(bool isCompleted, float timeSpent)
    {
        CourseData courseData = currentCourse < 5 ? sets[currentSet].subCourses[currentCourse] : sets[currentSet].bossCourse;
        float bestTime = courseData.bestTime;

        // Set the course data as completed and update the best time
        courseData.SetCourseCompleted(isCompleted);
        if (bestTime == 0 || timeSpent < bestTime)
            courseData.SetTimeSpent(timeSpent);

        // Save the data
        GameManager.instance.dataManager.SaveData(sets);

        // Load CourseSelectionMenu
        SceneManager.LoadScene(0);
    }

    public bool GetCompletionStatus(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].courseCompleted;
    }

    public bool GetCurrentCourseIsBossCourse()
    {
        return currentCourse < 5 ? false : true;
    }

    public string GetCourseName(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].courseName;
    }

    public string GetBossCourseName(int setIndex)
    {
        return sets[setIndex].bossCourse.courseName;
    }

    public float GetTimeSpent(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].bestTime;
    }

    public float GetBossTimeSpent(int setIndex)
    {
        return sets[setIndex].bossCourse.bestTime;
    }

    public float GetBossTimeLimit(int setIndex)
    {
        return sets[setIndex].bossTimeLimit;
    }

    public float GetTotalTimeSpent(int setIndex)
    {
        float totalTimeSpent = 0f;
        CourseData[] subcourses = sets[setIndex].subCourses;
        for (int i = 0; i < subcourses.Length; i++)
        {
            totalTimeSpent += subcourses[i].bestTime;
        }

        return totalTimeSpent;
    }

    public float GetCurrentSetTotalTimeSpent()
    {
        float totalTimeSpent = 0f;
        CourseData[] subcourses = sets[currentSet].subCourses;
        for (int i = 0; i < subcourses.Length; i++)
        {
            totalTimeSpent += subcourses[i].bestTime;
        }

        return totalTimeSpent;
    }

    public CourseData[] GetCourseData(int setIndex)
    {
        return sets[setIndex].subCourses;
    }

    public void SetCurrentSet(int setIndex)
    {
        currentSet = setIndex;
    }

    public void SetCurrentCourse(int courseIndex)
    {
        currentCourse = courseIndex;
    }
}

