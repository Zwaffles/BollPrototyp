using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField] private Timer timer;
    [SerializeField] private BossTimer bossTimer;
    [SerializeField] private GameObject pauseMenu;

    private InputReader input;
    private GameManager gameManager;

    private bool isPaused;
    private bool gameplayStarted;

    private void Start()
    {
        gameManager = GameManager.instance;

        input = gameManager.Input;

        input.HasMovedEvent += HandleHasMoved;

        input.PauseEvent += TogglePause;
        input.ResumeEvent += TogglePause;
    }

    public void StartGameplay()
    {
        timer.gameObject.SetActive(true);
        timer.ResetTimer();
        timer.StartTimer();

        if (gameManager.courseManager.GetCurrentCourseIsBossCourse())
        {
            bossTimer.gameObject.SetActive(true);
            bossTimer.ResetTimer();
            bossTimer.StartTimer();
        }
    }

    public void EndGameplay(bool completionStatus)
    {
        timer.StopTimer();
        GameManager.instance.courseManager.UpdateCourseData(completionStatus, timer.GetTimeSpent());
        timer.gameObject.SetActive(false);

        if (bossTimer.gameObject.activeInHierarchy)
        {
            bossTimer.StopTimer();
            bossTimer.gameObject.SetActive(false);
        }

        // Reset the gameplayStarted flag so that StartGameplay() can be called again for the next course.
        gameplayStarted = false;
    }

    private void HandleHasMoved()
    {
        if (!gameplayStarted)
        {
            gameplayStarted = true;
            StartGameplay();
        }
    }

    private void TogglePause()
    {
        if (gameManager.CurrentState != GameManager.GameState.Menu)
        {
            isPaused = !isPaused;
            gameManager.SetGameState(isPaused ? GameManager.GameState.Pause : GameManager.GameState.Play);
            Time.timeScale = isPaused ? 0f : 1f;
            pauseMenu.SetActive(isPaused);
        }
    }
}
