using JetBrains.Annotations;
using System;
using UnityEngine;

/// <summary>
/// The GameManager class controls the overall game flow and manages different game states.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader input;

    /// <summary>
    /// Gets the input reader for the GameManager.
    /// </summary>
    public InputReader Input
    {
        get
        {
            return input;
        }
    }

    /// <summary>
    /// Enumeration for different game states.
    /// </summary>
    public enum GameState
    {
        Play,
        Pause,
        Menu,
    }

    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager instance;

    public CourseManager courseManager { get; private set; }
    public DataManager dataManager { get; private set; }
    public UIManager uiManager { get; private set; }
    public AudioManager audioManager { get; private set; }

    [SerializeField] private GameState currentState = GameState.Menu;

    /// <summary>
    /// Gets or sets the current game state.
    /// </summary>
    public GameState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }

    /// <summary>
    /// Event triggered when the game state changes.
    /// </summary>
    public event Action<GameState> GameStateChangedEvent;

    private void Awake()
    {
        /// <summary>
        /// Ensures that only one instance of the GameManager exists.
        /// </summary>
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Find references to other managers in the scene
        courseManager = FindObjectOfType<CourseManager>();
        dataManager = FindObjectOfType<DataManager>();
        uiManager = FindObjectOfType<UIManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        /// <summary>
        /// Sets the input mode to UI.
        /// </summary>
        input.SetUI();
    }

    /// <summary>
    /// Sets the game state and triggers the event.
    /// </summary>
    /// <param name="newState">The new game state.</param>
    public void SetGameState(GameState newState)
    {
        currentState = newState;
        GameStateChangedEvent?.Invoke(newState);

        // Set the input mode based on the new game state
        if (newState == GameState.Play)
            input.SetGameplay();
        else
            input.SetUI();
    }
}