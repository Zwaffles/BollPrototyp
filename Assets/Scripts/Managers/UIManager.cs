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
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private LanguageMenu languageMenu;
    [SerializeField] private AudioMenu audioMenu;
    [SerializeField] private CreditScroll credits;
    [SerializeField] private VideoMenu videoMenu;

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

    private Color targetPrimaryColor, targetSecondaryColor;
    private Color currentPrimaryColor, currentSecondaryColor;
    private float colorTransitionSpeed = 1f;
    private UIColorManager colorManager;

    private void Start()
    {

        gameManager = GameManager.instance;
        courseManager = gameManager.courseManager;

        input = gameManager.Input;

        input.HasMovedEvent += HandleHasMoved;

        input.PauseEvent += TogglePause;
        input.ResumeEvent += TogglePause;

        currentPrimaryColor = new Color(0.2f,0.67f,0.24f);
        targetPrimaryColor = currentPrimaryColor;
        currentSecondaryColor = new Color(0f, 0.21f, 0.02f);
        targetSecondaryColor = currentSecondaryColor;

    }

    private void Update()
    {

        currentPrimaryColor = InterpolateColors(currentPrimaryColor, targetPrimaryColor, colorTransitionSpeed);
        currentSecondaryColor = InterpolateColors(currentSecondaryColor, targetSecondaryColor, colorTransitionSpeed);

        if (gameManager.CurrentState == GameManager.GameState.Menu)
        {

            try
            {
                colorManager.changeColors(currentPrimaryColor, currentSecondaryColor);
            }
            catch
            {
                Debug.Log("No color manager found.");
            }

        }

    }

    public void ProvideUIColorManager(UIColorManager uiColorManager)
    {
        colorManager = uiColorManager;
    }

    public void SetTargetColors(Color primaryColor, Color secondaryColor)
    {
        primaryColor.a = 1f;
        secondaryColor.a = 1f;

        targetPrimaryColor = primaryColor;
        targetSecondaryColor = secondaryColor;
    }

    private Color InterpolateColors(Color currentColor, Color targetColor, float interpolationSpeed)
    {

        if (currentColor != targetColor)
        {

            if (currentColor.r < targetColor.r)
            {
                currentColor.r += interpolationSpeed * Time.deltaTime;
                currentColor.r = currentColor.r > targetColor.r ? targetColor.r : currentColor.r;
            }
            else if (currentColor.r > targetColor.r)
            {
                currentColor.r -= interpolationSpeed * Time.deltaTime;
                currentColor.r = currentColor.r < targetColor.r ? targetColor.r : currentColor.r;
            }

            if (currentColor.g < targetColor.g)
            {
                currentColor.g += interpolationSpeed * Time.deltaTime;
                currentColor.g = currentColor.g > targetColor.g ? targetColor.g : currentColor.g;
            }
            else if (currentColor.g > targetColor.g)
            {
                currentColor.g -= interpolationSpeed * Time.deltaTime;
                currentColor.g = currentColor.g < targetColor.g ? targetColor.g : currentColor.g;
            }

            if (currentColor.b < targetColor.b)
            {
                currentColor.b += interpolationSpeed * Time.deltaTime;
                currentColor.b = currentColor.b > targetColor.b ? targetColor.b : currentColor.b;
            }
            else if (currentColor.b > targetColor.b)
            {
                currentColor.b -= interpolationSpeed * Time.deltaTime;
                currentColor.b = currentColor.b < targetColor.b ? targetColor.b : currentColor.b;
            }

        }

        return currentColor;

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

    public void ToggleVideoMenu(bool active)
    {
        videoMenu.gameObject.SetActive(active);
    }

    public void ToggleAudioMenu(bool active)
    {
        audioMenu.gameObject.SetActive(active);
    }

    public void ToggleLanguageMenu(bool active)
    {
        languageMenu.gameObject.SetActive(active);
    }

    public void ToggleCreditMenu(bool active)
    {
        credits.gameObject.SetActive(active);
    }

    public void ToggleOptionsMenu(bool active)
    {
        optionsMenu.gameObject.SetActive(active);
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
            float remainingTime = bossTimer.GetTimeLeft();
            bossTimer.gameObject.SetActive(false);

            if (completionStatus)
            {
                
                foreach(SetData set in courseManager.GetSetData())
                {
                    if (set.setName.Equals(courseManager.GetSetData()[courseManager.GetCurrentSet()].setUnlockedOnCompletion))
                        set.setUnlocked = true;
                }

                int stars = 1;

                SetData currentSet = courseManager.GetSetData()[courseManager.GetCurrentSet()];

                if (remainingTime >= currentSet.requiredTimeRemainingFor3StarScore)
                {
                    stars = 3;
                }
                else if (remainingTime >= currentSet.requiredTimeRemainingFor3StarScore / 2)
                {
                    stars = 2;
                }

                if (stars > currentSet.stars)
                {
                    currentSet.stars = stars;
                }

            }

        }

        // Reset the gameplayStarted flag so that StartGameplay() can be called again for the next course.
        gameplayStarted = false;

        endGameUI.gameObject.SetActive(true);
        endGameUI.DisplayStats(timeSpent, completionStatus);

        // Aloha achievement
        if (completionStatus && courseManager.GetCurrentSet() > 5 && courseManager.GetCurrentSet() < 9)
        {
            gameManager.achievementManager.GiveAchievement(Achievement.Aloha);
        }

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

            if (optionsMenu.gameObject.activeInHierarchy)
            {
                ToggleMainMenu(true);
                ToggleOptionsMenu(false);
            }

            if (videoMenu.gameObject.activeInHierarchy)
            {
                ToggleOptionsMenu(true);
                ToggleVideoMenu(false);
            }

            if (audioMenu.gameObject.activeInHierarchy)
            {
                ToggleOptionsMenu(true);
                ToggleAudioMenu(false);
            }

            if (languageMenu.gameObject.activeInHierarchy)
            {
                ToggleOptionsMenu(true);
                ToggleLanguageMenu(false);
            }

        }
    }

    public void handleBoost(int number)
    {
        useBoost.playBoostUIAnimation(number);
    }

}
