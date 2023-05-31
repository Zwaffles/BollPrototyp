using UnityEngine;

/// <summary>
/// Represents a set of courses within a game.
/// </summary>
[System.Serializable]
public class SetData
{
    /// <summary>
    /// The name of the set.
    /// </summary>
    public string setName;

    /// <summary>
    /// The total time spent in the set.
    /// </summary>
    public float totalTimeSpent { get; private set; }

    /// <summary>
    /// The time limit for completing the boss course.
    /// </summary>
    public float bossTimeLimit;

    /// <summary>
    /// The sub-courses within the set.
    /// </summary>
    public CourseData[] subCourses;

    /// <summary>
    /// The boss course within the set.
    /// </summary>
    public CourseData bossCourse;

    /// <summary>
    /// Creates a new SetData object with the specified parameters.
    /// </summary>
    /// <param name="setName">The name of the set.</param>
    /// <param name="bossTimeLimit">The time limit for completing the boss course.</param>
    /// <param name="subCourses">The sub-courses within the set.</param>
    /// <param name="bossCourse">The boss course within the set.</param>
    public SetData(string setName, float bossTimeLimit, CourseData[] subCourses, CourseData bossCourse)
    {
        this.setName = setName;
        this.bossTimeLimit = bossTimeLimit;
        this.subCourses = subCourses;
        this.bossCourse = bossCourse;
    }
}

/// <summary>
/// Represents a single course within a set.
/// </summary>
[System.Serializable]
public class CourseData
{
    /// <summary>
    /// The name of the course.
    /// </summary>
    public string courseName;

    /// <summary>
    /// The scene name associated with the course.
    /// </summary>
    public string sceneName;

    /// <summary>
    /// The par time for completing the course.
    /// </summary>
    public float parTime;

    /// <summary>
    /// Indicates whether the course has been completed.
    /// </summary>
    [HideInInspector]
    public bool courseCompleted = false;

    /// <summary>
    /// The best time achieved for completing the course.
    /// </summary>
    [HideInInspector]
    public float bestTime = 0f;

    /// <summary>
    /// Creates a new CourseData object with the specified parameters.
    /// </summary>
    /// <param name="courseName">The name of the course.</param>
    /// <param name="sceneName">The scene name associated with the course.</param>
    /// <param name="parTime">The par time for completing the course.</param>
    public CourseData(string courseName, string sceneName, float parTime)
    {
        this.courseName = courseName;
        this.sceneName = sceneName;
        this.parTime = parTime;
    }

    /// <summary>
    /// Sets the completion status of the course.
    /// </summary>
    /// <param name="isCompleted">The completion status of the course.</param>
    public void SetCourseCompleted(bool isCompleted)
    {
        courseCompleted = isCompleted;
    }

    /// <summary>
    /// Sets the best time achieved for completing the course.
    /// </summary>
    /// <param name="newBestTime">The new best time for completing the course.</param>
    public void SetTimeSpent(float newBestTime)
    {
        bestTime = newBestTime;
    }
}
