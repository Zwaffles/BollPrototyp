using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader input;

    public InputReader Input
    {
        get
        {
            return input;
        }
    }

    // Enumeration for different game states
    public enum GameState
    {
        Play,
        Pause,
        Menu,
    }

    // Singleton instance of the GameManager
    public static GameManager instance;

    // References to other managers in the game
    public CourseManager courseManager { get; private set; }
    public DataManager dataManager { get; private set; }
    public UIManager uiManager { get; private set; }
    public AudioManager audioManager { get; private set; }

    [SerializeField] private GameState currentState = GameState.Menu;

    // Property to get and set the current game state
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

    // Event triggered when the game state changes
    public event Action<GameState> GameStateChangedEvent;

    private void Awake()
    {
        // Ensure that only one instance of the GameManager exists
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
        // Set the input mode to UI
        input.SetUI();
    }

    // Method to set the game state and trigger the event
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
