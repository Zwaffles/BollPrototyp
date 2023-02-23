using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements"), SerializeField]
    private Timer timer;
    [SerializeField]
    private BossTimer bossTimer;
    [SerializeField]
    private GameObject pauseMenu;

    private InputReader input;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;

        input = gameManager.Input;

        input.PauseEvent += Pause;
        input.ResumeEvent += Resume;
    }

    public void StartGameplay()
    {
        timer.gameObject.SetActive(true);
        timer.ResetTimer();
        timer.StartTimer();

        if (!gameManager.courseManager.GetCurrentCourseIsBossCourse())
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

    public void Pause()
    {
        if (gameManager.CurrentGameState != GameManager.GameState.Play)
            return;

        gameManager.SetGameState(GameManager.GameState.Pause);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        if (gameManager.CurrentGameState != GameManager.GameState.Pause)
            return;

        gameManager.SetGameState(GameManager.GameState.Play);
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
