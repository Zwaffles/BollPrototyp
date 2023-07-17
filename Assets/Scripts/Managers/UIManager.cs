using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the UI elements and interactions during gameplay.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Menu UI Elements")]
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private SetSelectMenu setMenu;
    [SerializeField] private CourseSelect levelMenu;

    [Header("Game UI Elements")]
    [SerializeField] private Timer timer;
    [SerializeField] private UseBoost useBoost;
    [SerializeField] private BossTimer bossTimer;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private EndGameUI endGameUI;

    private InputReader input;
    private GameManager gameManager;
    private CourseManager courseManager;

    private bool isPaused;
    private bool gameplayStarted;

    private void Start()
    {
        gameManager = GameManager.instance;
        courseManager = gameManager.courseManager;

        input = gameManager.Input;

        input.HasMovedEvent += HandleHasMoved;

        input.PauseEvent += TogglePause;
        input.ResumeEvent += TogglePause;

    }

    public void ToggleMainMenu(bool active)
    {
        mainMenu.gameObject.SetActive(active);
    }

    public void ToggleSetSelectMenu(bool active)
    {
        setMenu.gameObject.SetActive(active);
    }

    public void ToggleLevelSelectMenu(bool active)
    {
        levelMenu.gameObject.SetActive(active);
    }

    public void ToggleLevelSelectMenu(bool active, int setNumber)
    {
        levelMenu.gameObject.SetActive(active);
        levelMenu.NavigateToSet(setNumber);
    }

    /// <summary>
    /// Starts the gameplay by activating and resetting the timer and boss timer (if applicable).
    /// </summary>
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

    /// <summary>
    /// Ends the gameplay and displays the end game UI with the provided completion status.
    /// </summary>
    /// <param name="completionStatus">The completion status of the gameplay.</param>
    public void EndGameplay(bool completionStatus)
    {
        gameManager.SetGameState(GameManager.GameState.Menu);
        timer.StopTimer();

        var timeSpent = timer.GetTimeSpent();

        courseManager.UpdateCourseData(
            courseManager.GetCurrentCourseCompletionStatus() ? true : completionStatus,
            completionStatus ? timeSpent : courseManager.GetCurrentCourseBestTime());
        timer.gameObject.SetActive(false);

        if (bossTimer.gameObject.activeInHierarchy)
        {
            bossTimer.StopTimer();
            bossTimer.gameObject.SetActive(false);

            if (completionStatus)
            {
                
                foreach(SetData set in courseManager.GetSetData())
                {
                    if (set.setName.Equals(courseManager.GetSetData()[courseManager.GetCurrentSet()].setUnlockedOnCompletion))
                        set.setUnlocked = true;
                }

            }

        }

        // Reset the gameplayStarted flag so that StartGameplay() can be called again for the next course.
        gameplayStarted = false;

        endGameUI.gameObject.SetActive(true);
        endGameUI.DisplayStats(timeSpent, completionStatus);
    }

    private void HandleHasMoved()
    {
        if (gameManager.CurrentState != GameManager.GameState.Play)
            return;
        if (gameplayStarted)
            return;

        gameplayStarted = true;
        StartGameplay();
    }

    private void TogglePause()
    {
        if (gameManager.CurrentState != GameManager.GameState.Menu)
        {
            // Placeholder quit-implementation
            EndGameplay(false);
            return;

            //isPaused = !isPaused;
            //gameManager.SetGameState(isPaused ? GameManager.GameState.Pause : GameManager.GameState.Play);
            //Time.timeScale = isPaused ? 0f : 1f;
            //pauseMenu.SetActive(isPaused);
        }
        else
        {
            //Placeholder quit-implementation

            if (mainMenu.gameObject.activeInHierarchy)
            {
                Application.Quit();
            }

            if (setMenu.gameObject.activeInHierarchy)
            {
                ToggleMainMenu(true);
                ToggleSetSelectMenu(false);
            }

            if (levelMenu.gameObject.activeInHierarchy)
            {
                ToggleSetSelectMenu(true);
                ToggleLevelSelectMenu(false);
            }

        }
    }

    public void handleBoost(int number)
    {
        useBoost.playBoostUIAnimation(number);
    }

}
