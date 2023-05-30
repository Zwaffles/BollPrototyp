using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class CourseManager : MonoBehaviour
{
    [SerializeField, Header("A list of SetData objects for each set of courses")]
    private List<SetData> sets;

    private int currentSet;
    private int currentCourse;
    private float parTime;

    private const float DEFAULT_TIME = 0f;

    // Load a specific course by setIndex and courseIndex
    public void LoadCourse(int setIndex, int courseIndex)
    {
        GameManager.instance.SetGameState(GameManager.GameState.Play);
        SceneManager.LoadScene(sets[setIndex].subCourses[courseIndex].sceneName);
        Debug.Log(GetCurrentParTime());
    }

    // Load the boss course for a specific set
    public void LoadBossCourse(int setIndex)
    {
        GameManager.instance.SetGameState(GameManager.GameState.Play);
        SceneManager.LoadScene(sets[setIndex].bossCourse.sceneName);
    }

    // Load player progress from a list of SetData
    public void LoadPlayerProgress(List<SetData> setDataList)
    {
        for (int i = 0; i < setDataList.Count; i++)
        {
            SetData setData = setDataList[i];
            SetData set = sets[i];

            for (int j = 0; j < set.subCourses.Length; j++)
            {
                set.subCourses[j].courseCompleted = setData.subCourses[j].courseCompleted;
                set.subCourses[j].bestTime = setData.subCourses[j].bestTime;
            }

            set.bossCourse.courseCompleted = setData.bossCourse.courseCompleted;
            set.bossCourse.bestTime = setData.bossCourse.bestTime;
        }
    }

    // Reset player progress for all courses
    public void ResetPlayerProgress()
    {
        foreach (SetData set in sets)
        {
            for (int i = 0; i < set.subCourses.Length; i++)
            {
                set.subCourses[i].courseCompleted = false;
                set.subCourses[i].bestTime = DEFAULT_TIME;
            }

            set.bossCourse.courseCompleted = false;
            set.bossCourse.bestTime = DEFAULT_TIME;
        }
    }

    // Update course data with completion status and time spent
    public void UpdateCourseData(bool isCompleted, float timeSpent)
    {
        CourseData courseData = currentCourse < 5 ? sets[currentSet].subCourses[currentCourse] : sets[currentSet].bossCourse;
        float bestTime = courseData.bestTime;

        // Set the course data as completed and update the best time
        courseData.SetCourseCompleted(isCompleted);
        if (isCompleted)
        {
            bestTime = bestTime == 0 || timeSpent < bestTime ? timeSpent : bestTime;
            courseData.SetTimeSpent(bestTime);
        }
    }

    // Get the completion status of a course
    public bool GetCompletionStatus(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].courseCompleted;
    }

    // Check if the current course is the boss course
    public bool GetCurrentCourseIsBossCourse()
    {
        return currentCourse == 5;
    }

    // Get the name of a set by setIndex
    public string GetSetName(int setIndex)
    {
        return sets[setIndex].setName;
    }

    // Get the name of a course by setIndex and courseIndex
    public string GetCourseName(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].courseName;
    }

    // Get the name of the boss course by setIndex
    public string GetBossCourseName(int setIndex)
    {
        return sets[setIndex].bossCourse.courseName;
    }

    // Get the time spent on a course by setIndex and courseIndex
    public float GetTimeSpent(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].bestTime;
    }

    // Get the time spent on the boss course by setIndex
    public float GetBossTimeSpent(int setIndex)
    {
        return sets[setIndex].bossCourse.bestTime;
    }

    // Get the time limit for the boss course by setIndex
    public float GetBossTimeLimit(int setIndex)
    {
        return sets[setIndex].bossTimeLimit;
    }

    // Get the total time spent on all sub-courses in a set by setIndex
    public float GetTotalTimeSpent(int setIndex)
    {
        return sets[setIndex].subCourses.Sum(course => course.bestTime);
    }

    // Get the remaining time limit for the current boss course
    public float GetCurrentBossTimeLimit()
    {
        float totalTimeSpent = GetTotalTimeSpent(currentSet);
        return sets[currentSet].bossTimeLimit - totalTimeSpent;
    }

    // Get the best time achieved on the current course
    public float GetCurrentCourseBestTime()
    {
        return currentCourse < 5 ? sets[currentSet].subCourses[currentCourse].bestTime : sets[currentSet].bossCourse.bestTime;
    }

    // Get the list of SetData objects
    public List<SetData> GetSetData()
    {
        return sets;
    }

    // Get the array of CourseData for a set by setIndex
    public CourseData[] GetCourseData(int setIndex)
    {
        return sets[setIndex].subCourses;
    }

    // Get the index of the current course
    public int GetCurrentCourse()
    {
        return currentCourse;
    }

    // Set the current set index
    public void SetCurrentSet(int setIndex)
    {
        currentSet = setIndex;
    }

    // Set the current course index
    public void SetCurrentCourse(int courseIndex)
    {
        currentCourse = courseIndex;
    }

    // Get the par time for the current course
    public float GetCurrentParTime()
    {
        return sets[currentSet].subCourses[currentCourse].parTime;
    }
}
