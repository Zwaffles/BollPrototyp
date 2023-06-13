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

    /// <summary>
    /// Load a specific course by setIndex and courseIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <param name="courseIndex">The index of the course within the set.</param>
    public void LoadCourse(int setIndex, int courseIndex)
    {

        SetData set = sets[setIndex];
        CourseData course = set.subCourses[courseIndex];

        GameManager.instance.SetGameState(GameManager.GameState.Play);
        SceneManager.LoadScene(course.sceneName);

        GameManager.instance.audioManager.PlayMusicWithOffset(set.musicName, course.parTime + set.musicParOffset);

        Debug.Log(GetCurrentParTime());
    }

    /// <summary>
    /// Load the boss course for a specific set.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    public void LoadBossCourse(int setIndex)
    {
        GameManager.instance.SetGameState(GameManager.GameState.Play);
        SceneManager.LoadScene(sets[setIndex].bossCourse.sceneName);
    }

    /// <summary>
    /// Load player progress from a list of SetData.
    /// </summary>
    /// <param name="setDataList">The list of SetData to load progress from.</param>
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

    /// <summary>
    /// Reset player progress for all courses.
    /// </summary>
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

    /// <summary>
    /// Update course data with completion status and time spent.
    /// </summary>
    /// <param name="isCompleted">Whether the course is completed or not.</param>
    /// <param name="timeSpent">The time spent on the course.</param>
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

    /// <summary>
    /// Get the completion status of a course.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <param name="courseIndex">The index of the course within the set.</param>
    /// <returns>True if the course is completed, false otherwise.</returns>
    public bool GetCompletionStatus(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].courseCompleted;
    }

    /// <summary>
    /// Get the completion status of the current course.
    /// </summary>
    /// <returns>True if the course is completed, false otherwise.</returns>
    public bool GetCurrentCourseCompletionStatus()
    {
        return sets[currentSet].subCourses[currentCourse].courseCompleted;
    }

    /// <summary>
    /// Check if the current course is the boss course.
    /// </summary>
    /// <returns>True if the current course is the boss course, false otherwise.</returns>
    public bool GetCurrentCourseIsBossCourse()
    {
        return currentCourse == 5;
    }

    /// <summary>
    /// Get the name of a set by setIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <returns>The name of the set.</returns>
    public string GetSetName(int setIndex)
    {
        return sets[setIndex].setName;
    }

    /// <summary>
    /// Get the name of a course by setIndex and courseIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <param name="courseIndex">The index of the course within the set.</param>
    /// <returns>The name of the course.</returns>
    public string GetCourseName(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].courseName;
    }

    /// <summary>
    /// Get the name of the boss course by setIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <returns>The name of the boss course.</returns>
    public string GetBossCourseName(int setIndex)
    {
        return sets[setIndex].bossCourse.courseName;
    }

    /// <summary>
    /// Get the time spent on a course by setIndex and courseIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <param name="courseIndex">The index of the course within the set.</param>
    /// <returns>The time spent on the course.</returns>
    public float GetTimeSpent(int setIndex, int courseIndex)
    {
        return sets[setIndex].subCourses[courseIndex].bestTime;
    }

    /// <summary>
    /// Get the time spent on the boss course by setIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <returns>The time spent on the boss course.</returns>
    public float GetBossTimeSpent(int setIndex)
    {
        return sets[setIndex].bossCourse.bestTime;
    }

    /// <summary>
    /// Get the time limit for the boss course by setIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <returns>The time limit for the boss course.</returns>
    public float GetBossTimeLimit(int setIndex)
    {
        return sets[setIndex].bossTimeLimit;
    }

    /// <summary>
    /// Get the total time spent on all sub-courses in a set by setIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <returns>The total time spent on all sub-courses in the set.</returns>
    public float GetTotalTimeSpent(int setIndex)
    {
        return sets[setIndex].subCourses.Sum(course => course.bestTime);
    }

    /// <summary>
    /// Get the remaining time limit for the current boss course.
    /// </summary>
    /// <returns>The remaining time limit for the current boss course.</returns>
    public float GetCurrentBossTimeLimit()
    {
        float totalTimeSpent = GetTotalTimeSpent(currentSet);
        return sets[currentSet].bossTimeLimit - totalTimeSpent;
    }

    /// <summary>
    /// Get the best time achieved on the current course.
    /// </summary>
    /// <returns>The best time achieved on the current course.</returns>
    public float GetCurrentCourseBestTime()
    {
        return currentCourse < 5 ? sets[currentSet].subCourses[currentCourse].bestTime : sets[currentSet].bossCourse.bestTime;
    }

    /// <summary>
    /// Get the list of SetData objects.
    /// </summary>
    /// <returns>The list of SetData objects.</returns>
    public List<SetData> GetSetData()
    {
        return sets;
    }

    /// <summary>
    /// Get the array of CourseData for a set by setIndex.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    /// <returns>The array of CourseData for the set.</returns>
    public CourseData[] GetCourseData(int setIndex)
    {
        return sets[setIndex].subCourses;
    }

    /// <summary>
    /// Get the index of the current course.
    /// </summary>
    /// <returns>The index of the current course.</returns>
    public int GetCurrentCourse()
    {
        return currentCourse;
    }

    /// <summary>
    /// Get the index of the current set.
    /// </summary>
    /// <returns>The index of the current set.</returns>
    public int GetCurrentSet()
    {
        return currentSet;
    }

    /// <summary>
    /// Set the current set index.
    /// </summary>
    /// <param name="setIndex">The index of the set.</param>
    public void SetCurrentSet(int setIndex)
    {
        currentSet = setIndex;
    }

    /// <summary>
    /// Set the current course index.
    /// </summary>
    /// <param name="courseIndex">The index of the course within the set.</param>
    public void SetCurrentCourse(int courseIndex)
    {
        currentCourse = courseIndex;
    }

    /// <summary>
    /// Get the par time for the current course.
    /// </summary>
    /// <returns>The par time for the current course.</returns>
    public float GetCurrentParTime()
    {
        return sets[currentSet].subCourses[currentCourse].parTime;
    }

    /// <summary>
    /// Get the amount of sets in the sets list.
    /// </summary>
    /// <returns>The count of the sets list.</returns>
    public int GetAmountOfSets()
    {
        return sets.Count();
    }
}
