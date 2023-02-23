using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements"), SerializeField]
    private Timer timer;
    [SerializeField]
    private BossTimer bossTimer;

    private void Start()
    {
       
    }

    public void StartGameplay()
    {
        timer.gameObject.SetActive(true);
        timer.ResetTimer();
        timer.StartTimer();

        if (!GameManager.instance.courseManager.GetCurrentCourseIsBossCourse())
            return;

        bossTimer.gameObject.SetActive(true);
        bossTimer.ResetTimer();
        bossTimer.StartTimer();
    }

    public void EndGameplay(bool completionStatus)
    {
        timer.StopTimer();
        // Set the course data as completed and update the time spent
        GameManager.instance.courseManager.UpdateCourseData(completionStatus, timer.GetTimeSpent());
        timer.gameObject.SetActive(false);

        if (!bossTimer.isActiveAndEnabled)
            return;

        bossTimer.StopTimer();
        bossTimer.gameObject.SetActive(false);
    }
}
