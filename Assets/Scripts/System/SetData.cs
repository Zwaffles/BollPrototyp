using UnityEngine;

[System.Serializable]
public class SetData
{
    public string setName;
    public float totalTimeSpent { get; private set; }
    public float bossTimeLimit;
    public CourseData[] subCourses;
    public CourseData bossCourse;

    public SetData(string setName, float bossTimeLimit, CourseData[] subCourses, CourseData bossCourse)
    {
        this.setName = setName;
        this.bossTimeLimit = bossTimeLimit;
        this.subCourses = subCourses;
        this.bossCourse = bossCourse;
    }
}

[System.Serializable]
public class CourseData
{
    public string courseName;
    public string sceneName;
    public float parTime;

    [HideInInspector]
    public bool courseCompleted = false;
    [HideInInspector]
    public float bestTime = 0f;

    public CourseData(string courseName, string sceneName, float parTime)
    {
        this.courseName = courseName;
        this.sceneName = sceneName;
        this.parTime = parTime;
    }

    public void SetCourseCompleted(bool isCompleted)
    {
        courseCompleted = isCompleted;
    }

    public void SetTimeSpent(float newBestTime)
    {
        bestTime = newBestTime;
    }
}
