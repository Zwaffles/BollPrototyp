using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CourseManager : MonoBehaviour
{
    [SerializeField, Header("A list of SetData objects for each set of courses")]
    private List<SetData> sets;

    public void LoadCourse(int setIndex, int courseIndex)
    {
        SceneManager.LoadScene(sets[setIndex].subCourses[courseIndex].sceneName);
    }

    public void LoadBossCourse(int setIndex)
    {
        SceneManager.LoadScene(sets[setIndex].bossCourse.sceneName);
    }

    public bool GetCompletionStatus(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].courseCompleted;
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
        return sets[setIndex].subCourses[courseIndex].timeSpent;
    }

    public float GetBossTimeSpent(int setIndex)
    {
        return sets[setIndex].bossCourse.timeSpent;
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
            totalTimeSpent += subcourses[i].timeSpent;
        }

        return totalTimeSpent;
    }

    public CourseData[] GetCourseData(int setIndex)
    {
        return sets[setIndex].subCourses;
    }
}

