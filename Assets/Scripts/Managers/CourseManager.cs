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

    public void LoadCourse(int setIndex, int courseIndex)
    {
        GameManager.instance.SetGameState(GameManager.GameState.Play);
        SceneManager.LoadScene(sets[setIndex].subCourses[courseIndex].sceneName);
        Debug.Log(GetCurrentParTime());
       
    }

    public void LoadBossCourse(int setIndex)
    {
        GameManager.instance.SetGameState(GameManager.GameState.Play);
        SceneManager.LoadScene(sets[setIndex].bossCourse.sceneName);
    }

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

    public bool GetCompletionStatus(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].courseCompleted;
    }

    public bool GetCurrentCourseIsBossCourse()
    {
        return currentCourse == 5;
    }

    public string GetSetName(int setIndex)
    {
        return sets[setIndex].setName;
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
        return sets[setIndex].subCourses.Sum(course => course.bestTime);
    }

    public float GetCurrentBossTimeLimit()
    {
        float totalTimeSpent = GetTotalTimeSpent(currentSet);
        return sets[currentSet].bossTimeLimit - totalTimeSpent;
    }

    public float GetCurrentCourseBestTime()
    {
        return currentCourse < 5 ? sets[currentSet].subCourses[currentCourse].bestTime : sets[currentSet].bossCourse.bestTime;
    }

    public List<SetData> GetSetData()
    {
        return sets;
    }

    public CourseData[] GetCourseData(int setIndex)
    {
        return sets[setIndex].subCourses;
    }

    public int GetCurrentCourse()
    {
        return currentCourse;
    }

    public void SetCurrentSet(int setIndex)
    {
        currentSet = setIndex;
    }

    public void SetCurrentCourse(int courseIndex)
    {
        currentCourse = courseIndex;
    }

    public float GetCurrentParTime()
    {
        return sets[currentSet].subCourses[currentCourse].parTime;
    }
}