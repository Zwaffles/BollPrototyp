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

    public bool courseCompleted { get; private set; } = false;
    public float timeSpent { get; private set; } = 0f;

    public CourseData(string courseName, string sceneName, bool courseCompleted, float timeSpent)
    {
        this.courseName = courseName;
        this.sceneName = sceneName;
        this.courseCompleted = courseCompleted;
        this.timeSpent = timeSpent;
    }
}
