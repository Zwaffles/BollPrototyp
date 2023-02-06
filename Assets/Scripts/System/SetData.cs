using UnityEngine;

[System.Serializable]
public class SetData
{
    public string setName;
    public float totalTimeSpent { get; private set; }
    public float bossTimeLimit;
    public CourseData[] subcourses;
    public CourseData bosscourse;

    public SetData(string setName, float bossTimeLimit, CourseData[] subcourses, CourseData bosscourse)
    {
        this.setName = setName;
        this.bossTimeLimit = bossTimeLimit;
        this.subcourses = subcourses;
        this.bosscourse = bosscourse;
    }
}

[System.Serializable]
public class CourseData
{
    public string courseName;
    public string sceneName;

    public bool courseCompleted = false;
    public float timeSpent = 0f;

    public CourseData(string courseName, string sceneName)
    {
        this.courseName = courseName;
        this.sceneName = sceneName;
    }
}
