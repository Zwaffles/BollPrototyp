using UnityEngine;
using System.Collections.Generic;

public class CourseManager : MonoBehaviour
{
    [SerializeField, Header("A list of SetData objects for each set of courses")]
    private List<SetData> sets;

    void Start()
    {
        // Initialize the sets
        sets = new List<SetData>();
    }

    public bool GetCompletionStatus(int setIndex, int courseIndex)
    {
        return sets[setIndex].subcourses[courseIndex].courseCompleted;
    }

    public string GetTimeSpent(int setIndex, int courseIndex)
    {
        float timeSpent = sets[setIndex].subcourses[courseIndex].timeSpent;
        return timeSpent > 0 ? timeSpent.ToString() : "course not yet completed";
    }
}

