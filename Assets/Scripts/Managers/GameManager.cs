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

    public enum GameState
    {
        Play,
        Pause,
        Menu,
    }

    public static GameManager instance;

    public CourseManager courseManager { get; private set; }
    public DataManager dataManager { get; private set; }
    public UIManager uiManager { get; private set; }
    public AudioManager audioManager { get; private set; }

    [SerializeField] private GameState currentState = GameState.Menu;

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
        input.SetUI();
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;
        GameStateChangedEvent?.Invoke(newState);

        if (newState == GameState.Play)
            input.SetGameplay();
        else
            input.SetUI();
    }
}
